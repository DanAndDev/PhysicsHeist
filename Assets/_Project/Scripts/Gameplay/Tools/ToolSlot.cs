using System;
using System.Collections.Generic;
using PhysicsHeist.Core.Input;
using PhysicsHeist.Core.Tools;
using UnityEngine;
using Zenject;

namespace PhysicsHeist.Gameplay.Tools
{
    /// <summary>
    /// Holds a player's set of tools and enforces "one equipped at a time".
    /// Listens to the input service for cycle (Tab / scroll) and direct-select
    /// (keys 1..N) commands.
    ///
    /// All tools are expected to be active in the prefab at scene-load time
    /// so Zenject's scene injector wires up their [Inject] dependencies
    /// before we disable them here — otherwise controllers added to an
    /// initially-inactive tool would fail to receive IInputService.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ToolSlot : MonoBehaviour
    {
        [SerializeField] private List<EquippableTool> tools = new();
        [SerializeField, Min(0)] private int startIndex;

        private IInputService _input;
        private int _currentIndex = -1;

        public event Action<IEquippable> Equipped;
        public event Action<IEquippable> Unequipped;

        public IReadOnlyList<EquippableTool> Tools => tools;
        public int CurrentIndex => _currentIndex;
        public IEquippable Current =>
            _currentIndex >= 0 && _currentIndex < tools.Count ? tools[_currentIndex] : null;

        [Inject]
        private void Construct(IInputService input)
        {
            _input = input;
        }

        private void Start()
        {
            if (tools.Count == 0)
            {
                _currentIndex = -1;
                return;
            }

            var initial = Mathf.Clamp(startIndex, 0, tools.Count - 1);
            for (var i = 0; i < tools.Count; i++)
            {
                var tool = tools[i];
                if (tool == null) continue;
                if (i == initial) tool.Equip();
                else tool.Unequip();
            }

            _currentIndex = initial;
            if (tools[initial] != null) Equipped?.Invoke(tools[initial]);
        }

        private void Update()
        {
            if (_input == null || tools.Count == 0) return;

            if (_input.ToolCyclePressedThisFrame) Cycle(1);

            var slot = _input.ToolSlotSelectedThisFrame;
            if (slot >= 0 && slot < tools.Count) EquipIndex(slot);
        }

        public void Cycle(int direction)
        {
            if (tools.Count == 0 || direction == 0) return;
            var step = direction > 0 ? 1 : -1;
            var next = _currentIndex;
            for (var i = 0; i < tools.Count; i++)
            {
                next = (next + step + tools.Count) % tools.Count;
                if (tools[next] != null)
                {
                    EquipIndex(next);
                    return;
                }
            }
        }

        public void EquipIndex(int index)
        {
            if (index == _currentIndex) return;
            if (index < 0 || index >= tools.Count) return;
            if (tools[index] == null) return;

            if (_currentIndex >= 0 && _currentIndex < tools.Count && tools[_currentIndex] != null)
            {
                var outgoing = tools[_currentIndex];
                outgoing.Unequip();
                Unequipped?.Invoke(outgoing);
            }

            _currentIndex = index;
            var incoming = tools[index];
            incoming.Equip();
            Equipped?.Invoke(incoming);
        }
    }
}
