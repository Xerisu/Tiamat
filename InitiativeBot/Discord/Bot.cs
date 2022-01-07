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
                await FindAndPrepareGuildTiamatChannel(guild, Strings.TiamatChannelName);
            }
        }

        #region Command resynchronization
        private IReadOnlyDictionary<string, SlashCommandBuilder> _commandToBuilders = new Dictionary<string, SlashCommandBuilder>()
        {
            [Strings.Commands.TiamatSetup.CommandName] = new SlashCommandBuilder()
                .WithName(Strings.Commands.TiamatSetup.CommandName)
                .WithDescription(Strings.Commands.TiamatSetup.CommandDescription),
            [Strings.Commands.TiamatHelp.CommandName] = new SlashCommandBuilder()
                .WithName(Strings.Commands.TiamatHelp.CommandName)
                .WithDescription(Strings.Commands.TiamatHelp.CommandDescription),
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

            var message = await channel.SendMessageAsync("This message is an intermediate step in configuring bot in this guild (2)");

            _loadedServers[guild.Id] = new ServerInfo(channel.Id, message.Id);
            Log.Information("Created info for guild {GuildName} ({GuildId}): ChannelId - {ChannelId}, MessageId - {MessagId}", guild.Name, guild.Id, channel.Id, message.Id);
        }

        private async Task<SocketTextChannel> CreateTiamatChannelInGuild(SocketGuild guild, string channelName)
        {
            var newChannel = await guild.CreateTextChannelAsync(channelName);

            Log.Information("Added channel {ChannelName} in guild {GuildName} ({GuildId})", channelName, guild.Name, guild.Id);
            return guild.GetTextChannel(newChannel.Id);
        }
        #endregion

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            try
            {
                switch (command.Data.Name)
                {
                    case Strings.Commands.TiamatSetup.CommandName:
                        await HandleSetupCommand(command);
                        break;
                    case Strings.Commands.TiamatHelp.CommandName:
                        await HandleHelpCommand(command);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Handling command {commandName} failed: {errorMessage}", command.CommandName, ex.Message);
                await command.RespondAsync($"Command {command.CommandName} failed :(", ephemeral: true);
            }
        }

        private async Task HandleSetupCommand(SocketSlashCommand command)
        {
            await FindAndPrepareGuildTiamatChannel(((SocketGuildChannel)command.Channel).Guild, Strings.TiamatChannelName);
            await command.RespondAsync(Strings.Commands.TiamatSetup.SetupResponseMessage);
        }

        private async Task HandleHelpCommand(SocketSlashCommand command)
        {
            await command.RespondAsync(Strings.Commands.TiamatHelp.HelpResponseMessage, ephemeral: true);
        }
    }
}
