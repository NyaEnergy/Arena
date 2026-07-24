using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class StoryMissionView : MonoBehaviour {
    [Header("Task")]
    [SerializeField] private GameObject _taskPanel;
    [SerializeField] private TMP_Text _taskTitle;
    [SerializeField] private TMP_Text _taskDescription;
    [SerializeField] private TMP_Text _taskProgress;
    [SerializeField] private Image _taskProgressFill;

    [Header("Dialogue")]
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private Image _dialoguePortrait;
    [SerializeField] private TMP_Text _dialogueSpeaker;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _skipButton;

    [Header("Result")]
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private TMP_Text _resultTitle;
    [SerializeField] private TMP_Text _resultDescription;
    [SerializeField] private Button _resultButton;
    [SerializeField] private TMP_Text _resultButtonLabel;

    public Button NextButton => _nextButton;
    public Button SkipButton => _skipButton;
    public Button ResultButton => _resultButton;

    public bool IsValid =>
        _taskPanel != null &&
        _taskTitle != null &&
        _taskDescription != null &&
        _taskProgress != null &&
        _taskProgressFill != null &&
        _dialoguePanel != null &&
        _dialoguePortrait != null &&
        _dialogueSpeaker != null &&
        _dialogueText != null &&
        _nextButton != null &&
        _skipButton != null &&
        _resultPanel != null &&
        _resultTitle != null &&
        _resultDescription != null &&
        _resultButton != null &&
        _resultButtonLabel != null;

    public void HideAll() {
        _taskPanel.SetActive(false);
        _dialoguePanel.SetActive(false);
        _resultPanel.SetActive(false);
    }

    public void RenderTask(string title,
                           string description,
                           string progress,
                           float normalizedProgress) {
        _dialoguePanel.SetActive(false);
        _resultPanel.SetActive(false);
        _taskPanel.SetActive(true);

        _taskTitle.text = title;
        _taskDescription.text = description;
        _taskProgress.text = progress;
        _taskProgressFill.fillAmount =
            Mathf.Clamp01(normalizedProgress);
    }

    public void RenderDialogue(string speakerName,
                               Sprite portrait,
                               string text) {
        _taskPanel.SetActive(false);
        _resultPanel.SetActive(false);
        _dialoguePanel.SetActive(true);

        _dialogueSpeaker.text = speakerName;
        _dialogueText.text = text;
        _dialoguePortrait.sprite = portrait;
        _dialoguePortrait.gameObject.SetActive(
            portrait != null);
    }

    public void RenderResult(string title,
                             string description,
                             string buttonLabel) {
        _taskPanel.SetActive(false);
        _dialoguePanel.SetActive(false);
        _resultPanel.SetActive(true);

        _resultTitle.text = title;
        _resultDescription.text = description;
        _resultButtonLabel.text = buttonLabel;
    }
}
