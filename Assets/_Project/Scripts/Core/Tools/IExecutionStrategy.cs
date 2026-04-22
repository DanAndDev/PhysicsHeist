namespace PhysicsHeist.Core.Tools
{
    public interface IExecutionStrategy
    {
        void Execute(in ToolContext context, in ToolTarget target);
    }
}
