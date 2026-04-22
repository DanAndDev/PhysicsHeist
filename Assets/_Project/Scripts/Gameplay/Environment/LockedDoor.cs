using System;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Environment
{
    public sealed class LockedDoor : MonoBehaviour
    {
        [SerializeField] private Transform doorTransform;
        [SerializeField] private Vector3 closedLocalPosition;
        [SerializeField] private Vector3 openLocalPosition = new(0f, 3f, 0f);
        [SerializeField, Min(0f)] private float openSpeed = 2f;
        [SerializeField] private bool startOpen;
        [SerializeField] private bool startLocked;

        private bool isOpen;
        private bool isLocked;
        private float openness;

        public bool IsOpen => isOpen;
        public bool IsLocked => isLocked;
        public float Openness => openness;

        public event Action<LockedDoor> Opened;
        public event Action<LockedDoor> Closed;
        public event Action<LockedDoor, bool> LockedChanged;

        private void Awake()
        {
            if (doorTransform == null) doorTransform = transform;
            isOpen = startOpen;
            isLocked = startLocked;
            openness = isOpen ? 1f : 0f;
            ApplyOpenness();
        }

        public void Open()
        {
            if (isLocked || isOpen) return;
            isOpen = true;
            Opened?.Invoke(this);
        }

        public void Close()
        {
            if (!isOpen) return;
            isOpen = false;
            Closed?.Invoke(this);
        }

        public void SetOpen(bool open)
        {
            if (open) Open(); else Close();
        }

        public void SetLocked(bool locked)
        {
            if (isLocked == locked) return;
            isLocked = locked;
            LockedChanged?.Invoke(this, locked);
            if (locked && isOpen) Close();
        }

        private void Update()
        {
            var target = isOpen ? 1f : 0f;
            if (Mathf.Approximately(openness, target)) return;

            openness = Mathf.MoveTowards(openness, target, openSpeed * Time.deltaTime);
            ApplyOpenness();
        }

        private void ApplyOpenness()
        {
            if (doorTransform == null) return;
            doorTransform.localPosition = Vector3.Lerp(closedLocalPosition, openLocalPosition, openness);
        }
    }
}
