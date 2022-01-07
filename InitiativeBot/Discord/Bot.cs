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

        public Bot()
        {
            _client = new DiscordSocketClient();
            _client.Log += Logging;
            _client.Ready += ResyncBot;
            _client.SlashCommandExecuted += SlashCommandHandler;
        }

        public async Task Main()
        {
            var token = "OTI5MDcyODIxMDYwMTM3MDUw.YdiAfQ.R1LjsDLynwV0JnqReXOF61G9fN0";

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
            var commands = await guild.GetApplicationCommandsAsync();
            commands ??= new List<SocketApplicationCommand>();

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

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch(command.Data.Name)
            {
                case Strings.Commands.TiamatSetup.CommandName:
                    await HandleSetupCommand(command);
                    break;
                case Strings.Commands.TiamatHelp.CommandName:
                    await HandleHelpCommand(command);
                    break;
            }
        }

        private async Task HandleSetupCommand(SocketSlashCommand command)
        {
            await command.RespondAsync("a");
        }

        private async Task HandleHelpCommand(SocketSlashCommand command)
        {
            await command.RespondAsync(Strings.Commands.TiamatHelp.HelpResponseMessage, ephemeral: true);
        }
    }
}
