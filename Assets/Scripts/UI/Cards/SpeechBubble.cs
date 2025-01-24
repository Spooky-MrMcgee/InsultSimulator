using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] float typewriterSpeed = 0.1f;
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
        if(!gameObject.activeInHierarchy || sentencePieces.Count == 0)
        {
            layout.enabled = false;
            textArea.SetText("");
            layout.enabled = true;
            LayoutRebuilder.ForceRebuildLayoutImmediate(layout.transform as RectTransform);
            return;
        }

        StopAllCoroutines();
        StartCoroutine(TypewriteAnimation(finished));
    }

    IEnumerator TypewriteAnimation(bool finished)
    {
        var sentence = string.Join(" ", sentencePieces);
        sentence += finished ? "!" : "...";

        var charactersToGoBack = finished ? sentence.Length - 1 : sentence.Length - sentencePieces[^1].Length;

        for (int i = charactersToGoBack; i < sentence.Length; i++)
        {
            var content = sentence.Substring(0, i + 1);

            layout.enabled = false;
            textArea.SetText(content);
            layout.enabled = true;
            LayoutRebuilder.ForceRebuildLayoutImmediate(layout.transform as RectTransform);

            yield return new WaitForSeconds(typewriterSpeed);
        }
    }
}
