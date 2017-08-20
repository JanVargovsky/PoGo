﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using PoGo.DiscordBot.Commands;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PoGo.DiscordBot
{
    public class PoGoBot : IDisposable
    {
        const string Token = "MzQ3ODM2OTIyMDM2NzQ4Mjg5.DHeNAg.X7SXUjVVWteb14T9ewdULDFBB0A";
        public const char Prefix = '!';

        readonly LogSeverity LogSeverity;
        readonly DiscordSocketClient client;
        readonly CommandService commands;

        public PoGoBot()
        {
            LogSeverity = LogSeverity.Debug;
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity,
            });
            commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity,
            });

            client.Log += Log;
            client.LoggedIn += LoggedIn;
            client.MessageReceived += HandleCommand;
            client.ReactionAdded += RaidCommand.OnReactionAdded;
            client.ReactionRemoved += RaidCommand.OnReactionRemoved;
        }

        public void Dispose()
        {
            client?.LogoutAsync().Wait();
            client?.Dispose();
        }

        public async Task RunAsync()
        {
            await InitCommands();

            await client.LoginAsync(TokenType.Bot, Token);
            await client.StartAsync();
        }

        async Task InitCommands()
        {
            var modules = await commands.AddModulesAsync(Assembly.GetEntryAssembly());
            foreach (var module in modules)
                Console.WriteLine($"{module.Name}: {string.Join(", ", module.Commands.Select(t => t.Name))}");
        }

        async Task HandleCommand(SocketMessage messageParam)
        {
            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;
            // Determine if the message is a command, based on if it starts with '!' or a mention prefix
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) return;
            // Create a Command Context
            var context = new CommandContext(client, message);
            // Execute the command. (result does not indicate a return value, 
            // rather an object stating if the command executed succesfully)
            var result = await commands.ExecuteAsync(context, argPos);
            if (!result.IsSuccess)
                Console.WriteLine(result.ErrorReason);
            //await context.Channel.SendMessageAsync(result.ErrorReason);
        }

        async Task ReactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel socketMessageChannel, SocketReaction socket)
        {
            if (RaidCommand.ActiveRaids.TryGetValue(message.Id, out var raidInfo))
            {
                IUserMessage raidMessage = await message.GetOrDownloadAsync();
                if (socket.Emote.Name == Emojis.ThumbsUp)
                {
                    raidInfo.Users.Add(socket.User.Value);
                    await raidMessage.ModifyAsync(t => t.Embed = raidInfo.ToEmbed());
                }
                else
                    await raidMessage.RemoveReactionAsync(socket.Emote, socket.User.Value);
            }
        }

        Task LoggedIn()
        {
            Console.WriteLine("Logged in");
            return Task.CompletedTask;
        }

        Task Log(LogMessage message)
        {
            var cc = Console.ForegroundColor;
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message}");
            Console.ForegroundColor = cc;
            return Task.CompletedTask;
        }

        Task MessageReceived(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message == null) return Task.CompletedTask;
            if (message.Content.StartsWith("!hovno"))
                message.AddReactionAsync(new Emoji("💩"));

            Console.WriteLine($"Received: {arg}");
            return Task.CompletedTask;
        }
    }
}