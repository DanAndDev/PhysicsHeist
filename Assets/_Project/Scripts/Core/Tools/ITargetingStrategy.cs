namespace PhysicsHeist.Core.Tools
{
    public interface ITargetingStrategy
    {
        ToolTarget Resolve(in ToolContext context);
    }
}
