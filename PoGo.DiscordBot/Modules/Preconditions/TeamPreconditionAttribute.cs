﻿using Discord.Commands;
using Discord.WebSocket;
using PoGo.DiscordBot.Services;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace PoGo.DiscordBot.Modules.Preconditions
{
    public class TeamPreconditionAttribute : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (!(context.User is SocketGuildUser guildUser))
                return Task.FromResult<PreconditionResult>(TeamPreconditionResult.Fail);

            var userService = services.GetService<UserService>();
            var team = userService.GetTeam(guildUser);

            if(team == null)
                return Task.FromResult<PreconditionResult>(TeamPreconditionResult.Fail);

            return Task.FromResult<PreconditionResult>(TeamPreconditionResult.Success);
        }
    }

    public class TeamPreconditionResult : PreconditionResult
    {
        protected TeamPreconditionResult(CommandError? error, string errorReason)
            : base(error, errorReason)
        {
        }

        public static TeamPreconditionResult Success => new TeamPreconditionResult(null, null);
        public static TeamPreconditionResult Fail => new TeamPreconditionResult(CommandError.UnmetPrecondition, "Je nutné si zvolit team.");
    }
}
