using System.Collections.Generic;

namespace ConveyorWars.Units {
    public sealed class ActiveAllyGroupRuntime {
        private const int MAX_ACTIVE_COUNT = 3;

        private readonly List<UnitEntity> _active = new();
        private readonly Queue<UnitConfig> _reserve = new();

        public IReadOnlyList<UnitEntity> Active => _active;
        public UnitEntity Leader { get; private set; }

        public int ActiveCount => _active.Count;
        public int ReserveCount => _reserve.Count;
        public bool HasFreeActiveSlot => _active.Count < MAX_ACTIVE_COUNT;

        public bool TryAddActive(UnitEntity unit) {
            if (unit == null ||
                unit.Side != UnitSide.Rubezh ||
                !HasFreeActiveSlot ||
                _active.Contains(unit)) {
                return false;
            }

            _active.Add(unit);

            if (Leader == null) {
                Leader = unit;
            }

            return true;
        }

        public bool TryRemoveActive(UnitEntity unit) {
            if (unit == null ||
                !_active.Remove(unit)) {
                return false;
            }

            if (Leader == unit) {
                Leader = null;
            }

            return true;
        }

        public bool TryAddReserve(UnitConfig config) {
            if (config == null ||
                config.Side != UnitSide.Rubezh) {
                return false;
            }

            _reserve.Enqueue(config);
            return true;
        }

        public bool TryPeekReserve(out UnitConfig config) {
            config = null;

            if (_reserve.Count == 0) {
                return false;
            }

            config = _reserve.Peek();
            return config != null;
        }

        public bool TryTakeReserve(out UnitConfig config) {
            config = null;

            if (_reserve.Count == 0) {
                return false;
            }

            config = _reserve.Dequeue();
            return config != null;
        }

        public bool TrySetLeader(UnitEntity unit) {
            if (!IsActive(unit)) return false;

            Leader = unit;
            return true;
        }

        public bool IsActive(UnitEntity unit) {
            return unit != null &&
                   _active.Contains(unit);
        }

        public bool IsLeader(UnitEntity unit) {
            return unit != null &&
                   Leader == unit;
        }

        public bool IsAssistant(UnitEntity unit) {
            return IsActive(unit) &&
                   !IsLeader(unit);
        }

        public int GetAssistantSlot(UnitEntity unit) {
            if (!IsAssistant(unit)) return -1;

            int slot = 0;

            for (int i = 0; i < _active.Count; i++) {
                UnitEntity active = _active[i];

                if (active == Leader) {
                    continue;
                }

                if (active == unit) {
                    return slot;
                }

                slot++;
            }

            return -1;
        }
    }
}