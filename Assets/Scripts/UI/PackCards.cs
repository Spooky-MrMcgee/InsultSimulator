using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackCards : MonoBehaviour
{
    [SerializeField] RectTransform displayArea;

    List<UICard> cardUIs = new();

    private void Start()
    {
        ShopManager.Instance.PackOpened += DisplayCards;
    }

    private void OnDestroy()
    {
        ShopManager.Instance.PackOpened -= DisplayCards;
    }

    void DisplayCards(List<CardData> cards)
    {
        foreach (var card in cards)
        {
            var ui = CardSpawner.Instance.SpawnCard(card, false);
            ui.SetParent(displayArea);
            cardUIs.Add(ui);
        }

        Invoke(nameof(RemoveCards), 2);
    }

    void RemoveCards()
    {
        foreach (var card in cardUIs)
        {
            CardSpawner.Instance.DespawnCard(card);
        }

        cardUIs.Clear();
    }
}
