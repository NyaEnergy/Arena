using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class CampaignArcOptionView : MonoBehaviour {
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _status;
    [SerializeField] private GameObject _selection;

    public Button Button => _button;
    public StoryArcConfig StoryArc { get; private set; }

    public void Render(StoryArcConfig storyArc,
                       string status,
                       Color statusColor,
                       bool isSelected,
                       bool isInteractable) {

        StoryArc = storyArc;
        gameObject.SetActive(storyArc != null);

        if (storyArc == null) return;

        if (_title != null) {
            _title.text = storyArc.DisplayName;
        }

        if (_status != null) {
            _status.text = status;
            _status.color = statusColor;
        }

        if (_selection != null) {
            _selection.SetActive(isSelected);
        }

        if (_button != null) {
            _button.interactable = isInteractable;
        }
    }

    public void Hide() {
        StoryArc = null;
        gameObject.SetActive(false);
    }
}
