using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Campaign/Story Task Config",
                 fileName = "StoryTaskConfig")]
public sealed class StoryTaskConfig : ScriptableObject {
    [Header("Identity")]
    [SerializeField] private string _id;
    [SerializeField] private string _title;
    [SerializeField, TextArea] private string _description;

    [Header("Objective")]
    [SerializeField] private StoryTaskObjectiveType _objectiveType;
    [SerializeField, Min(1)] private int _targetAmount = 1;

    [Header("Dialogue")]
    [SerializeField] private DialogueConfig _introDialogue;
    [SerializeField] private DialogueConfig _outroDialogue;

    public string Id =>
        string.IsNullOrWhiteSpace(_id) ?
        string.Empty :
        _id.Trim();

    public string Title =>
        string.IsNullOrWhiteSpace(_title) ?
        name :
        _title.Trim();

    public string Description => _description;
    public StoryTaskObjectiveType ObjectiveType => _objectiveType;
    public int TargetAmount => Mathf.Max(1, _targetAmount);
    public DialogueConfig IntroDialogue => _introDialogue;
    public DialogueConfig OutroDialogue => _outroDialogue;

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Id) &&
        _targetAmount > 0 &&
        _introDialogue != null &&
        _introDialogue.IsValid &&
        _outroDialogue != null &&
        _outroDialogue.IsValid;
}
