using Discord.Net;
using Discord.WebSocket;
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
            _client.SlashCommandExecuted += SlashCommandHandler;
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
                await ResyncCommandsInGuild(guild);
                await FindAndPrepareGuildTiamatChannel(guild, Constatns.TiamatChannelName);
                await UpdateMessageForGuild(guild);
            }
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
            var channel = guild.TextChannels.FirstOrDefault(c => c.Name == tiamatChannelName);

            if(channel == null)
            {
                channel = await CreateTiamatChannelInGuild(guild, tiamatChannelName);
            }
            var messages = await channel.GetMessagesAsync(50).FlattenAsync();
            await channel.DeleteMessagesAsync(messages);
            Log.Information("Removed {MessageCount} messages from the channel in {GuildName} ({GuildId})", messages.Count(), guild.Name, guild.Id);

            var messageId = await SendIntermediateSetupMessage(channel);

            _loadedServers[guild.Id] = new ServerInfo(channel.Id, messageId);
            Log.Information("Created info for guild {GuildName} ({GuildId}): ChannelId - {ChannelId}, MessageId - {MessagId}", guild.Name, guild.Id, channel.Id, messageId);
        }

        private async Task<ulong> SendIntermediateSetupMessage(SocketTextChannel channel)
        {
            var emoji = String.IsNullOrWhiteSpace(Constatns.Message.Button.Emote) ? null : new Emoji(Constatns.Message.Button.Emote);
            var buttonBuilder = new ComponentBuilder()
                .WithButton(Constatns.Message.Button.Label, Constatns.Message.Button.Id, Constatns.Message.Button.Style, emoji);
            var message = await channel.SendMessageAsync(Constatns.Message.IntermediateStepMessage, components: buttonBuilder.Build());
            return message.Id;
        }

        private async Task<SocketTextChannel> CreateTiamatChannelInGuild(SocketGuild guild, string channelName)
        {
            var newChannel = await guild.CreateTextChannelAsync(channelName);

            Log.Information("Added channel {ChannelName} in guild {GuildName} ({GuildId})", channelName, guild.Name, guild.Id);
            return guild.GetTextChannel(newChannel.Id);
        }
        #endregion

        private async Task UpdateMessageForGuild(SocketGuild guild)
        {
            if(!_loadedServers.TryGetValue(guild.Id, out var serverInfo))
            {
                throw new UserMessageException("Server was not properly configured. Try to run `/create-channel`.");
            }

            var channel = guild.GetTextChannel(serverInfo.ChannelId);
            await channel.ModifyMessageAsync(serverInfo.MessageId, messageProperties =>
            {
                messageProperties.Content = serverInfo.GetDiscordMessage();
            });
        }

        // Returns message that should be sent to the user as a result
        private string HandleUserException(Exception ex, string context)
        {
            Log.Error(ex, "{Context} failed: {errorMessage}", context, ex.Message);
            return ex is UserMessageException umex ? umex.Message : "Unknown error :(";
        }

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
            await command.RespondAsync(Constatns.Commands.TiamatSetup.SetupResponseMessage);
        }

        private async Task HandleHelpCommand(SocketSlashCommand command)
        {
            await command.RespondAsync(Constatns.Commands.TiamatHelp.HelpResponseMessage, ephemeral: true);
        }
    }
}
