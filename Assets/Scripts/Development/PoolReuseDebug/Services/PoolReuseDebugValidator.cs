using UnityEngine;

public class PoolReuseDebugValidator {
    private readonly BattlefieldRegistry _battlefieldRegistry;

    public PoolReuseDebugValidator(BattlefieldRegistry battlefieldRegistry) {
        _battlefieldRegistry = battlefieldRegistry;
    }

    public PoolReuseDebugValidationResult Validate(BattlefieldCharacter character) {
        if (character == null) {
            return new PoolReuseDebugValidationResult(false, "CharacterFactory не вернул противника.");
        }

        CharacterBrain brain = character.Brain;

        if (brain.Runtime.IsDead.CurrentValue) {
            return new PoolReuseDebugValidationResult(false, "Противник сохранил состояние уничтожения.");
        }

        if (!Mathf.Approximately(brain.Runtime.CurrentHP.CurrentValue,
                                 brain.Config.MaxHP)) {
            return new PoolReuseDebugValidationResult(false, "Здоровье противника не восстановилось полностью.");
        }

        if (brain.TargetComponent.CurrentTarget.CurrentValue != null) {
            return new PoolReuseDebugValidationResult(false, "Противник сохранил старую цель.");
        }

        if (_battlefieldRegistry.Enemies.Count != 1) {
            return new PoolReuseDebugValidationResult(false, "BattlefieldRegistry содержит неверное количество противников.");
        }

        if (!character.gameObject.activeInHierarchy) {
            return new PoolReuseDebugValidationResult(false, "Повторно созданный противник неактивен.");
        }
            
        return new PoolReuseDebugValidationResult(true, string.Empty);
    }
}