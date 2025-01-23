using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textArea;
    [SerializeField] LayoutGroup layout;

    List<string> sentencePieces = new();

    public void AddTo(string text)
    {
        sentencePieces.Add(text);
        UpdateText(false);
    }

    public void RemoveFrom(string text)
    {
        sentencePieces.Remove(text);
        UpdateText(false);
    }

    public void SetFinished()
    {
        UpdateText(true);
    }

    public void ToggleVisibility(bool on)
    {
        gameObject.SetActive(on);
    }

    public void ResetText()
    {
        sentencePieces.Clear();
        UpdateText(false);
    }

    public void UpdateText(bool finished)
    {
        layout.enabled = false;
        var sentence = string.Join(" ", sentencePieces);

        sentence += finished ? "!" : "...";
        textArea.SetText(sentence);
        layout.enabled = true;
        LayoutRebuilder.ForceRebuildLayoutImmediate(layout.transform as RectTransform);
    }
}
