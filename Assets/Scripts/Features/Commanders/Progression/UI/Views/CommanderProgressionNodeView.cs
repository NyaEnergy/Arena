using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class CommanderProgressionNodeView : MonoBehaviour {
    [SerializeField] private GameObject _unlockedMark;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _quest;
    [SerializeField] private TMP_Text _progressText;
    [SerializeField] private TMP_Text _status;
    [SerializeField] private Image _progressFill;

    public void Render(string title,
                       string description,
                       string quest,
                       float normalizedProgress,
                       string progressText,
                       string status,
                       Color statusColor,
                       bool isUnlocked) {

        gameObject.SetActive(true);

        if (_title != null) {
            _title.text = title;
        }

        if (_description != null) {
            _description.text = description;
        }

        if (_quest != null) {
            _quest.text = quest;
        }

        if (_progressFill != null) {
            _progressFill.fillAmount =
                Mathf.Clamp01(normalizedProgress);
        }

        if (_progressText != null) {
            _progressText.text = progressText;
        }

        if (_status != null) {
            _status.text = status;
            _status.color = statusColor;
        }

        if (_unlockedMark != null) {
            _unlockedMark.SetActive(isUnlocked);
        }
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
