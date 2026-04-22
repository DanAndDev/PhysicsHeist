using System;

namespace PhysicsHeist.Core.Physics
{
    public interface IBreakable
    {
        float StructuralHealth { get; }
        float MaxStructuralHealth { get; }
        bool IsBroken { get; }

        void ApplyDamage(in BreakageInfo info);

        event Action<IBreakable, BreakageInfo> Broken;
    }
}
