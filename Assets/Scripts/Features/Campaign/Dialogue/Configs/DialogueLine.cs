using System;
using UnityEngine;

[Serializable]
public sealed class DialogueLine {
    [SerializeField] private DialogueSpeakerType _speakerType;
    [SerializeField] private string _speakerNameOverride;
    [SerializeField] private Sprite _portraitOverride;
    [SerializeField, TextArea] private string _text;

    public DialogueSpeakerType SpeakerType => _speakerType;

    public string SpeakerNameOverride =>
        string.IsNullOrWhiteSpace(_speakerNameOverride) ?
        string.Empty :
        _speakerNameOverride.Trim();

    public Sprite PortraitOverride => _portraitOverride;
    public string Text => _text;

    public bool IsValid {
        get {
            if (string.IsNullOrWhiteSpace(_text)) return false;

            return _speakerType != DialogueSpeakerType.Custom ||
                   (!string.IsNullOrWhiteSpace(
                        SpeakerNameOverride) &&
                    _portraitOverride != null);
        }
    }
}
