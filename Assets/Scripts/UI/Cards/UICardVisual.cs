using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICardVisual : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI content;

    [SerializeField]
    Button button;

    [SerializeField]
    Image background;

    Action onClick;

    public void Setup(CardData card)
    {
        content.SetText(card.content);
    }

    public void SetAction(Action onClick)
    {
        this.onClick = onClick;
    }

    public void AnimateToNewPosition(Vector3 position)
    {
        GetComponent<RectTransform>().position = position;
    }

    public void OnSelected()
    {
        onClick?.Invoke();
    }
}
