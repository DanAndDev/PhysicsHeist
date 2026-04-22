using PhysicsHeist.Core.Physics;

namespace PhysicsHeist.Core.Signals
{
    public readonly struct ObjectBrokenSignal
    {
        public readonly IBreakable Breakable;
        public readonly BreakageInfo Info;

        public ObjectBrokenSignal(IBreakable breakable, BreakageInfo info)
        {
            Breakable = breakable;
            Info = info;
        }
    }
}
