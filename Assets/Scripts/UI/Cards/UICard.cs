using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICard : MonoBehaviour
{
    public CardData Data { get; private set; }
    public UICardVisual card;

    public void Setup(CardData data, UICardVisual visual, bool showPrice)
    {
        Data = data;
        card = visual;

        card.Setup(Data, showPrice);
    }

    public void SetOnSelected(Action onSelected)
    {
        card.SetAction(onSelected);
    }

    public void SetParent(RectTransform parent)
    {
        transform.SetParent(parent);
        GetComponent<RectTransform>().position = parent.position;
        RefreshPosition();
    }

    public void SetInteractable(bool on)
    {
        card.SetInteractable(on);
    }

    public void RefreshPosition()
    {
        StartCoroutine(AnimateVisual());
    }

    IEnumerator AnimateVisual()
    {
        yield return null;
        yield return null;
        card.AnimateToNewPosition(GetComponent<RectTransform>().position);
    }
}
