using UnityEngine;

namespace PhysicsHeist.Core.Config
{
    public abstract class ConfigAsset : ScriptableObject
    {
        private void OnValidate()
        {
            Validate();
        }

        protected virtual void Validate()
        {
        }
    }
}
