using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI content;

    [SerializeField]
    Button button;

    [SerializeField]
    Image background;

    public void Setup(CardData card)
    {
        content.SetText(card.content);
    }

    public void SetAction(Action onClick)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick());
    }
}
