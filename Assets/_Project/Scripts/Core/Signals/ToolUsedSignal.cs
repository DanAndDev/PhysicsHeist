using PhysicsHeist.Core.Tools;

namespace PhysicsHeist.Core.Signals
{
    public readonly struct ToolUsedSignal
    {
        public readonly IToolAction Tool;
        public readonly ToolUseResult Result;
        public readonly ToolTarget Target;

        public ToolUsedSignal(IToolAction tool, ToolUseResult result, ToolTarget target)
        {
            Tool = tool;
            Result = result;
            Target = target;
        }
    }
}
