using Discord.Net;
using Discord.WebSocket;
using InitiativeBot.Commands;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    internal class Bot
    {
        private readonly DiscordSocketClient _client;
        private readonly Dictionary<ulong, ServerInfo> _loadedServers = new();

        public Bot()
        {
            _client = new DiscordSocketClient();
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
            foreach(var guild in _client.Guilds)
            {
                await ResyncBotInGuild(guild);
            }
        }

        private async Task ResyncBotInGuild(SocketGuild guild)
        {
            await ResyncCommandsInGuild(guild);
            await FindAndPrepareGuildTiamatChannel(guild, Constatns.TiamatChannelName);
            await RunCommandForGuild(guild);
        }

        #region Command resynchronization
        private IReadOnlyDictionary<string, SlashCommandBuilder> _commandToBuilders = new Dictionary<string, SlashCommandBuilder>()
        {
            [Constatns.Commands.TiamatSetup.CommandName] = new SlashCommandBuilder()
                .WithName(Constatns.Commands.TiamatSetup.CommandName)
                .WithDescription(Constatns.Commands.TiamatSetup.CommandDescription),
            [Constatns.Commands.TiamatHelp.CommandName] = new SlashCommandBuilder()
                .WithName(Constatns.Commands.TiamatHelp.CommandName)
                .WithDescription(Constatns.Commands.TiamatHelp.CommandDescription),
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
                Log.Information("Added {Command} command in guild {GuildName} ({GuildId})", command, guild.Name, guild.Id);
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
            var messages = await channel.GetMessagesAsync(50).FlattenAsync();
            await channel.DeleteMessagesAsync(messages);
            Log.Information("Removed {MessageCount} message(s) from the channel in {GuildName} ({GuildId})", messages.Count(), guild.Name, guild.Id);

            var messageId = await SendIntermediateSetupMessage(channel);

            _loadedServers[guild.Id] = new ServerInfo(channel.Id, messageId);
            Log.Information("Created info for guild {GuildName} ({GuildId}): ChannelId - {ChannelId}, MessageId - {MessagId}", guild.Name, guild.Id, channel.Id, messageId);
        }

        private async Task<ulong> SendIntermediateSetupMessage(ITextChannel channel)
        {
            var emoji = String.IsNullOrWhiteSpace(Constatns.Message.Button.Emote) ? null : new Emoji(Constatns.Message.Button.Emote);
            var buttonBuilder = new ComponentBuilder()
                .WithButton(Constatns.Message.Button.Label, Constatns.Message.Button.Id, Constatns.Message.Button.Style, emoji);
            var message = await channel.SendMessageAsync(Constatns.Message.IntermediateStepMessage, components: buttonBuilder.Build());
            return message.Id;
        }

        private async Task<ITextChannel> CreateTiamatChannelInGuild(SocketGuild guild, string channelName)
        {
            var newChannel = await guild.CreateTextChannelAsync(channelName);

            Log.Information("Added channel {ChannelName} in guild {GuildName} ({GuildId})", channelName, guild.Name, guild.Id);
            return newChannel;
        }
        #endregion

        // If command is null - message will be only updated
        private async Task RunCommandForGuild(SocketGuild guild, ICommand? command = null)
        {
            if(!_loadedServers.TryGetValue(guild.Id, out var serverInfo))
                throw new UserMessageException(Constatns.Error.ReconfigureServerErrorMessage);

            var channel = guild.GetTextChannel(serverInfo.ChannelId);

            if(channel == null)
                throw new UserMessageException(Constatns.Error.ReconfigureServerErrorMessage);

            if(await channel.GetMessageAsync(serverInfo.MessageId) == null)
                throw new UserMessageException(Constatns.Error.ReconfigureServerErrorMessage);

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

        // Returns message that should be sent to the user as a result
        private string HandleUserException(Exception ex, string context)
        {
            Log.Error(ex, "{Context} failed: {errorMessage}", context, ex.Message);
            return ex is UserMessageException umex ? umex.Message : Constatns.Error.UnknownErrorMessage;
        }

        #region Slash commands handling 
        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            try
            {
                switch (command.Data.Name)
                {
                    case Constatns.Commands.TiamatSetup.CommandName:
                        await HandleSetupCommand(command);
                        break;
                    case Constatns.Commands.TiamatHelp.CommandName:
                        await HandleHelpCommand(command);
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
            await FindAndPrepareGuildTiamatChannel(((SocketGuildChannel)command.Channel).Guild, Constatns.TiamatChannelName);
            await command.RespondAsync(Constatns.Commands.TiamatSetup.ResponseMessage);
        }

        private async Task HandleHelpCommand(SocketSlashCommand command)
        {
            await command.RespondAsync(Constatns.Commands.TiamatHelp.ResponseMessage, ephemeral: true);
        }

        #endregion

        #region Button handling
        private async Task ButtonHandler(SocketMessageComponent button)
        {
            try
            {
                switch (button.Data.CustomId)
                {
                    case Constatns.Message.Button.Id:
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
