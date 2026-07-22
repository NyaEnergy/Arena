using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Campaign/Story Territory Config",
                 fileName = "StoryTerritoryConfig")]
public sealed class StoryTerritoryConfig : ScriptableObject {
    [SerializeField] private string _id;
    [SerializeField] private string _displayName;
    [SerializeField] private string _sceneName;
    [SerializeField] private List<StoryTaskConfig> _storyTasks = new();

    public string Id => string.IsNullOrWhiteSpace(_id)
        ? string.Empty
        : _id.Trim();

    public string DisplayName =>
        string.IsNullOrWhiteSpace(_displayName) ?
        name : _displayName.Trim();

    public string SceneName =>
        string.IsNullOrWhiteSpace(_sceneName) ?
        string.Empty : _sceneName.Trim();

    public IReadOnlyList<StoryTaskConfig> StoryTasks => _storyTasks;

    public bool IsValid {
        get {
            if (string.IsNullOrWhiteSpace(Id) ||
                string.IsNullOrWhiteSpace(SceneName) ||
                _storyTasks == null ||
                _storyTasks.Count == 0) return false;

            for (int i = 0; i < _storyTasks.Count; ++i) {
                if (_storyTasks[i] == null ||
                    !_storyTasks[i].IsValid) return false;
            }

            return true;
        }
    }
}
