using System.Collections.Generic;
using ConveyorWars.Presentation.Units;
using ConveyorWars.Units;

namespace ConveyorWars.Application {
    public sealed class UnitRegistry {
        private readonly List<UnitInstance> _units = new();
        private readonly Dictionary<UnitView, UnitInstance> _byView = new();
        private readonly Dictionary<UnitEntity, UnitInstance> _byEntity = new();

        public IReadOnlyList<UnitInstance> Units => _units;

        public bool TryRegister(UnitInstance unit) {
            if (unit == null ||
                unit.View == null ||
                unit.Entity == null ||
                _byView.ContainsKey(unit.View) ||
                _byEntity.ContainsKey(unit.Entity)) {
                return false;
            }

            _units.Add(unit);
            _byView.Add(unit.View, unit);
            _byEntity.Add(unit.Entity, unit);

            return true;
        }

        public bool TryGet(UnitView view, out UnitInstance unit) {
            unit = null;

            return view != null &&
                   _byView.TryGetValue(view, out unit);
        }

        public bool TryGet(UnitEntity entity, out UnitInstance unit) {
            unit = null;

            return entity != null &&
                   _byEntity.TryGetValue(entity, out unit);
        }

        public bool TryUnregister(UnitInstance unit) {
            if (unit == null) return false;

            _units.Remove(unit);

            bool removedView = unit.View != null &&
                              _byView.Remove(unit.View);

            bool removedEntity = unit.Entity != null &&
                                _byEntity.Remove(unit.Entity);

            return removedView || removedEntity;
        }
    }
}