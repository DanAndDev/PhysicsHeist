using UnityEngine;

namespace PhysicsHeist.Core.Tools
{
    public readonly struct ToolContext
    {
        public readonly Vector3 Origin;
        public readonly Vector3 Direction;
        public readonly float Range;
        public readonly GameObject Owner;
        public readonly LayerMask LayerMask;

        public ToolContext(Vector3 origin, Vector3 direction, float range, GameObject owner, LayerMask layerMask)
        {
            Origin = origin;
            Direction = direction.sqrMagnitude > 1e-6f ? direction.normalized : Vector3.forward;
            Range = range;
            Owner = owner;
            LayerMask = layerMask;
        }
    }
}
