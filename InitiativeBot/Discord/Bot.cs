﻿using Discord.Net;
using Discord.WebSocket;
using InitiativeBot.Commands;
using InitiativeBot.Parser;
using InitiativeBot.Parser.JoinModifier;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Discord
{
    internal class Bot
    {
        private const int _synchronizationDelayBetweenGuildsInMs = 500; // Needed to not reach api rate limit
        private static readonly DiscordSocketConfig _discordSocketConfig = new()
        {
            GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages
        };


        private readonly DiscordSocketClient _client;
        private readonly Dictionary<ulong, ServerInfo> _loadedServers = new();

        public Bot()
        {
            _client = new DiscordSocketClient(_discordSocketConfig);
            _client.Log += Logging;
            _client.Ready += ResyncBot;
            _client.JoinedGuild += ResyncBotInGuild;
            _client.SlashCommandExecuted += SlashCommandHandler;
            _client.ButtonExecuted += ButtonHandler;
        }

        public async Task Main(string token)
        {
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Logging(LogMessage msg)
        {
            Log.Information(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task ResyncBot()
        {
            Task[] resyncGuildTasks = new Task[_client.Guilds.Count];
            Log.Information( "Bot present in {Count} guilds", _client.Guilds.Count );
            for (int i = 0; i < _client.Guilds.Count; i++)
            {
                resyncGuildTasks[i] = ResyncBotInGuild(_client.Guilds.ElementAt(i));
                await Task.Delay(_synchronizationDelayBetweenGuildsInMs); // Without any delay Discord API complains about rate limit 
            }
            await Task.WhenAll(resyncGuildTasks);
        }

        private async Task ResyncBotInGuild(SocketGuild guild)
        {
            Log.Information("Started synchronization within guild {GuildName} ({GuildId})", guild.Name, guild.Id);
            
            Task resyncCommands = ResyncCommandsInGuild(guild);
            Task prepareChannel = FindAndPrepareGuildTiamatChannel(guild, Constants.BotSettings.ChannelName);
            await Task.WhenAll(resyncCommands, prepareChannel);
            
            await RunCommandForGuild(guild);
            
            Log.Information("Finished synchronization within guild {GuildName} ({GuildId})", guild.Name, guild.Id);
        }

        #region Command resynchronization
        private readonly IReadOnlyDictionary<string, SlashCommandBuilder> _commandToBuilders = new Dictionary<string, SlashCommandBuilder>()
        {
            [Constants.Commands.TiamatSetup.CommandName] = new SlashCommandBuilder()
                .WithName(Constants.Commands.TiamatSetup.CommandName)
                .WithDescription(Constants.Commands.TiamatSetup.CommandDescription),
            [Constants.Commands.TiamatHelp.CommandName] = new SlashCommandBuilder()
                .WithName(Constants.Commands.TiamatHelp.CommandName)
                .WithDescription(Constants.Commands.TiamatHelp.CommandDescription),
            [Constants.Commands.TiamatClear.CommandName] = new SlashCommandBuilder()
                .WithName(Constants.Commands.TiamatClear.CommandName)
                .WithDescription(Constants.Commands.TiamatClear.CommandDescription),
            [Constants.Commands.TiamatRemove.CommandName] = new SlashCommandBuilder()
                .WithName(Constants.Commands.TiamatRemove.CommandName)
                .WithDescription(Constants.Commands.TiamatRemove.CommandDescription)
                .AddOption(
                    Constants.Commands.TiamatRemove.PlayerNameParameterName, 
                    ApplicationCommandOptionType.String, 
                    Constants.Commands.TiamatRemove.PlayerNameParameterDescription,
                    isRequired: true),
            [Constants.Commands.TiamatJoin.CommandName] = new SlashCommandBuilder()
                .WithName(Constants.Commands.TiamatJoin.CommandName)
                .WithDescription(Constants.Commands.TiamatJoin.CommandDescription)
                .AddOption(
                    Constants.Commands.TiamatJoin.PlayerNameParameterName,
                    ApplicationCommandOptionType.String,
                    Constants.Commands.TiamatJoin.PlayerNameParameterDescription,
                    isRequired: true)
                .AddOption(
                    Constants.Commands.TiamatJoin.ModifiersParameterName,
                    ApplicationCommandOptionType.String,
                    Constants.Commands.TiamatJoin.ModifiersParameterDescription,
                    isRequired: false),
        };

        private async Task ResyncCommandsInGuild(SocketGuild guild)
        {
            await guild.DeleteApplicationCommandsAsync();
            foreach (var command in _commandToBuilders.Keys)
            {
                await AddCommandInGuild(command, guild);
            }
        }

        private async Task AddCommandInGuild(string command, SocketGuild guild)
        {
            try
            {
                var builder = _commandToBuilders[command];
                await _client.Rest.CreateGuildCommand(builder.Build(), guild.Id);
                Log.Debug("Added {Command} command in guild {GuildName} ({GuildId})", command, guild.Name, guild.Id);
            } 
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Log.Error(json);
            }
        }
        #endregion

        #region Channel resynchronization
        private async Task FindAndPrepareGuildTiamatChannel(SocketGuild guild, string tiamatChannelName)
        {
            ITextChannel? channel = guild.TextChannels.FirstOrDefault(c => c.Name == tiamatChannelName);

            if(channel == null)
            {
                channel = await CreateTiamatChannelInGuild(guild, tiamatChannelName);
            }

            var messages = await channel.GetMessagesAsync().FlattenAsync();
            var messagesPossibleToDelete = messages.Where( message => ( DateTimeOffset.UtcNow - message.Timestamp ).TotalDays <= 14 );
            await channel.DeleteMessagesAsync( messagesPossibleToDelete );
            Log.Debug("Removed {MessageCount} message(s) from the channel in {GuildName} ({GuildId})", messagesPossibleToDelete.Count(), guild.Name, guild.Id);

            messages = await channel.GetMessagesAsync().FlattenAsync();
            if(messages.Any())
            {
                await SendRemoveOldMessagesWarning( channel );
            }

            var messageId = await SendIntermediateSetupMessage(channel);

            _loadedServers[guild.Id] = new ServerInfo(channel.Id, messageId);
            Log.Information("Created info for guild {GuildName} ({GuildId}): ChannelId - {ChannelId}, MessageId - {MessagId}", guild.Name, guild.Id, channel.Id, messageId);
        }

        private static async Task<ulong> SendIntermediateSetupMessage(ITextChannel channel)
        {
            var emoji = String.IsNullOrWhiteSpace(Constants.Message.Button.Emote) ? null : new Emoji(Constants.Message.Button.Emote);
            var buttonBuilder = new ComponentBuilder()
                .WithButton(Constants.Message.Button.Label, Constants.Message.Button.Id, Constants.Message.Button.Style, emoji);
            var message = await channel.SendMessageAsync(Constants.Message.IntermediateStepMessage, components: buttonBuilder.Build());
            return message.Id;
        }

        private static async Task<ulong> SendRemoveOldMessagesWarning( ITextChannel channel )
        {
            var message = await channel.SendMessageAsync( Constants.Message.OldMessagesWarning );
            return message.Id;
        }

        private static async Task<ITextChannel> CreateTiamatChannelInGuild(SocketGuild guild, string channelName)
        {
            var newChannel = await guild.CreateTextChannelAsync(channelName);

            Log.Information("Added channel {ChannelName} in guild {GuildName} ({GuildId})", channelName, guild.Name, guild.Id);
            return newChannel;
        }
        #endregion

        #region Utility
        // If command is null - message will be only updated
        private async Task RunCommandForGuild(SocketGuild guild, ICommand? command = null)
        {
            if(!_loadedServers.TryGetValue(guild.Id, out var serverInfo))
                throw new UserMessageException(Constants.Error.ReconfigureServerErrorMessage);

            var channel = guild.GetTextChannel(serverInfo.ChannelId);

            if(channel == null)
                throw new UserMessageException(Constants.Error.ReconfigureServerErrorMessage);

            if(await channel.GetMessageAsync(serverInfo.MessageId) == null)
                throw new UserMessageException(Constants.Error.ReconfigureServerErrorMessage);

            if(command != null)
            {
                Log.Information("Command {Command} run for {GuildName} ({GuildId})", command, guild.Name, guild.Id);
                serverInfo.InitiativeList.ExecuteCommand(command);
            }
                

            await channel.ModifyMessageAsync(serverInfo.MessageId, messageProperties =>
            {
                messageProperties.Content = serverInfo.GetDiscordMessage();
            });
        }

        private static T GetRequiredParameterFromCommand<T>(SocketSlashCommand command, string parameterName, out ApplicationCommandOptionType type)
        {
            var parameter = command.Data.Options.FirstOrDefault(option => option.Name == parameterName);

            if (parameter == null)
            {
                throw new UserMessageException(string.Format(Constants.Error.MissingParameterErrorMessage, parameterName));   
            }

            type = parameter.Type;
            return (T)parameter.Value;
        }

        private static T? GetOptionalParameterFromCommand<T>(SocketSlashCommand command, string parameterName, out ApplicationCommandOptionType? type)
        {
            var parameter = command.Data.Options.FirstOrDefault(option => option.Name == parameterName);

            if (parameter == null)
            {
                type = null;
                return default;
            }

            type = parameter.Type;
            return (T)parameter.Value;
        }

        // Returns message that should be sent to the user as a result
        private static string HandleUserException(Exception ex, string context)
        {
            Log.Error(ex, "{Context} failed: {errorMessage}", context, ex.Message);
            return ex is UserMessageException umex ? umex.Message : Constants.Error.UnknownErrorMessage;
        }
        #endregion

        #region Slash commands handling 
        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            try
            {
                switch (command.Data.Name)
                {
                    case Constants.Commands.TiamatSetup.CommandName:
                        await HandleSetupCommand(command);
                        break;
                    case Constants.Commands.TiamatHelp.CommandName:
                        await HandleHelpCommand(command);
                        break;
                    case Constants.Commands.TiamatClear.CommandName:
                        await HandleClearCommand(command);
                        break;
                    case Constants.Commands.TiamatRemove.CommandName:
                        await HandleRemoveCommand(command);
                        break;
                    case Constants.Commands.TiamatJoin.CommandName:
                        await HandleJoinCommand(command);
                        break;
                }
            }
            catch (Exception ex)
            {
                string message = HandleUserException(ex, command.CommandName);
                await command.RespondAsync(message, ephemeral: true);
            }
        }

        private async Task HandleSetupCommand(SocketSlashCommand command)
        {
            await FindAndPrepareGuildTiamatChannel(((SocketGuildChannel)command.Channel).Guild, Constants.BotSettings.ChannelName);
            await command.RespondAsync(Constants.Commands.TiamatSetup.ResponseMessage);
        }

        private static async Task HandleHelpCommand(SocketSlashCommand command)
        {
            await command.RespondAsync(Constants.Commands.TiamatHelp.ResponseMessage, ephemeral: true);
        }

        private async Task HandleClearCommand(SocketSlashCommand command)
        {
            var guild = ((SocketGuildChannel)command.Channel).Guild;
            await RunCommandForGuild(guild, new ClearCommand());
            await command.RespondAsync(Constants.Commands.TiamatClear.ResponseMessage, ephemeral: true);
        }

        private async Task HandleRemoveCommand(SocketSlashCommand command)
        {
            var guild = ((SocketGuildChannel)command.Channel).Guild;
            string playerName = GetRequiredParameterFromCommand<string>(command, Constants.Commands.TiamatRemove.PlayerNameParameterName, out _);

            await RunCommandForGuild(guild, new RemoveCommand(playerName));
            await command.RespondAsync(string.Format(Constants.Commands.TiamatRemove.ResponseMessage, playerName), ephemeral: true);
        }

        private async Task HandleJoinCommand(SocketSlashCommand command)
        {
            var guild = ((SocketGuildChannel)command.Channel).Guild;
            string playerName = GetRequiredParameterFromCommand<string>(command, Constants.Commands.TiamatJoin.PlayerNameParameterName, out _);
            string modifiers = GetOptionalParameterFromCommand<string>(command, Constants.Commands.TiamatJoin.ModifiersParameterName, out _) ?? String.Empty;
            modifiers = Regex.Replace(modifiers, @"\s", "");

            IJoinModifier[] parsedModifiers;

            try
            {
                parsedModifiers = Parser.ParseJoinModifiersString(modifiers);
                await RunCommandForGuild(guild, new JoinCommand(playerName, Constants.BotSettings.DefaultDie, parsedModifiers));
            }
            catch(ArgumentException)
            {
                throw new UserMessageException(Constants.Error.InvalidJoinModifiersErrorMessage);
            }
            

            if (String.IsNullOrWhiteSpace(modifiers))
                modifiers = "<None>"; //Just for nicer response

            await command.RespondAsync(string.Format(Constants.Commands.TiamatJoin.ResponseMessage, playerName, modifiers), ephemeral: true);
        }

        #endregion

        #region Button handling
        private async Task ButtonHandler(SocketMessageComponent button)
        {
            try
            {
                switch (button.Data.CustomId)
                {
                    case Constants.Message.Button.Id:
                        await HandleNextTurnButton(button);
                        break;
                }
            }
            catch (Exception ex)
            {
                string message = HandleUserException(ex, button.Data.CustomId);
                await button.RespondAsync(message, ephemeral: true);
            }
        }

        private async Task HandleNextTurnButton(SocketMessageComponent button)
        {
            var guild = ((SocketGuildChannel)button.Channel).Guild;
            await RunCommandForGuild(guild, new NextTurnCommand());
            await button.UpdateAsync(messageProperties => { });
        }
        #endregion
    }
}
