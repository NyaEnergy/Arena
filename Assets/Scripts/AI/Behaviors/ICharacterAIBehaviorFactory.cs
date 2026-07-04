public interface ICharacterAIBehaviorFactory {
    bool CanCreate(CharacterBrain brain);
    ICharacterAIBehavior Create(CharacterBrain brain);
}