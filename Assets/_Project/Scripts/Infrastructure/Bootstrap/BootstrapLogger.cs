using UnityEngine;

namespace PhysicsHeist.Infrastructure.Bootstrap
{
    internal static class BootstrapLogger
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBoot()
        {
            Debug.Log("[PhysicsHeist] boot ok");
        }
    }
}
