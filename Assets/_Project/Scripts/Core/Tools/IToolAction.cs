using System;

namespace PhysicsHeist.Core.Tools
{
    public interface IToolAction
    {
        string DisplayName { get; }
        bool IsReady { get; }

        ToolUseResult TryUse(in ToolContext context);

        event Action<IToolAction, ToolUseResult, ToolTarget> Used;
    }
}
