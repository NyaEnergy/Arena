public interface ICharacterBehaviorFactory {
    bool CanCreate(CharacterBrain brain);
    ICharacterBehavior Create(CharacterBrain brain);
}