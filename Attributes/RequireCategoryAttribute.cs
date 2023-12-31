﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Collections.ObjectModel;


namespace Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class RequireCategoryAttribute : CheckBaseAttribute
    {
        public IReadOnlyList<string> ChannelNames { get; }
        public ChannelCheckMode CheckMode { get; }
        public RequireCategoryAttribute(ChannelCheckMode checkMode, params string[] channelNames)
        {
            ChannelNames = new ReadOnlyCollection<string>(channelNames);
            CheckMode = checkMode;
        }

        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            if (ctx.Guild == null || ctx.Member == null)
                return Task.FromResult(false);

            bool contains = ChannelNames.Contains(ctx.Channel.Parent.Name, StringComparer.OrdinalIgnoreCase);

            switch (CheckMode)
            {
                case ChannelCheckMode.Any:
                    return Task.FromResult(contains);
                case ChannelCheckMode.None:
                    return Task.FromResult(!contains);
                case ChannelCheckMode.MineOrParentAny:
                    return Task.FromResult(false);
                default:
                    return Task.FromResult(false);
            }
        }
    }
}
