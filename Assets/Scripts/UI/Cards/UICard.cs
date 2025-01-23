using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICard : MonoBehaviour
{
    public CardData Data { get; private set; }
    public UICardVisual card;

    public void Setup(CardData data, UICardVisual visual)
    {
        Data = data;
        card = visual;

        card.Setup(Data);
    }

    public void SetOnSelected(Action onSelected)
    {
        card.SetAction(onSelected);
    }

    public void SetParent(RectTransform parent)
    {
        transform.SetParent(parent);
        RefreshPosition();
    }

    public void RefreshPosition()
    {
        StartCoroutine(AnimateVisual());
    }

    IEnumerator AnimateVisual()
    {
        yield return new WaitForSeconds(0.1f);
        card.AnimateToNewPosition(GetComponent<RectTransform>().position);
    }
}
