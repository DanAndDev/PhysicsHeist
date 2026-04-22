using UnityEngine;

namespace PhysicsHeist.Core.Physics
{
    public readonly struct BreakageInfo
    {
        public readonly Vector3 Point;
        public readonly float Magnitude;
        public readonly GameObject Source;

        public BreakageInfo(Vector3 point, float magnitude, GameObject source = null)
        {
            Point = point;
            Magnitude = magnitude;
            Source = source;
        }
    }
}
