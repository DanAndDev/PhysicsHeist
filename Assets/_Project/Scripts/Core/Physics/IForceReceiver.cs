using UnityEngine;

namespace PhysicsHeist.Core.Physics
{
    public interface IForceReceiver
    {
        void ApplyForce(Vector3 force, ForceMode mode = ForceMode.Force);
        void ApplyForceAtPoint(Vector3 force, Vector3 worldPoint, ForceMode mode = ForceMode.Force);
        void ApplyRadialForce(Vector3 origin, float magnitude, float radius, ForceMode mode = ForceMode.Impulse);
    }
}
