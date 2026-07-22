using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Campaign/Story Task Config",
                 fileName = "StoryTaskConfig")]
public sealed class StoryTaskConfig : ScriptableObject {
    [SerializeField] private string _id;
    [SerializeField] private string _title;
    [SerializeField, TextArea] private string _description;

    public string Id =>
        string.IsNullOrWhiteSpace(_id) ?
        string.Empty : _id.Trim();

    public string Title =>
        string.IsNullOrWhiteSpace(_title) ?
        name : _title.Trim();

    public string Description => _description;
    public bool IsValid => !string.IsNullOrWhiteSpace(Id);
}
