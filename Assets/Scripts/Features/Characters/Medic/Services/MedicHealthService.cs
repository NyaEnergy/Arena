public class MedicHealthService {
    private const float HEALTH_EPSILON = 0.0001f;

    public float GetHealthPercent(CharacterBrain character) {
        if(character == null ||
           character.Config.MaxHP <= 0f) {
            return 0f;
        }

        return character.Runtime.CurrentHP.CurrentValue /
               character.Config.MaxHP;
    }

    public bool IsWounded(CharacterBrain character) {
        if(character == null) return false;
        return character.Runtime.CurrentHP.CurrentValue <
               character.Config.MaxHP - HEALTH_EPSILON;
    }
}
