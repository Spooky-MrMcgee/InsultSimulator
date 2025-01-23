using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsultUIManager : MonoBehaviour
{
    [SerializeField] RectTransform fieldRoot;
    [SerializeField] RectTransform handRoot;

    List<UICard> cards = new();

    private void Start()
    {
        CardManager.Instance.StateChanged += OnInsultStateChanged;
    }

    private void OnDestroy()
    {
        CardManager.Instance.StateChanged -= OnInsultStateChanged;
    }

    void OnPlayerChanged(CardManager.PlayerSelection player)
    {
    }

    void OnInsultStateChanged(CardManager.CardStates state, List<CardData> availableCards)
    {
        DiscardCurrentCards();

        foreach (var card in availableCards)
        {
            var cardUI = CardSpawner.Instance.SpawnCard(card);
            cardUI.SetParent(handRoot);

            cardUI.SetOnSelected(() =>
            {
                DisableCurrentCards();
                RefreshCurrentCards();

                CardManager.Instance.SelectCard(cardUI.Data);
                CardManager.Instance.PlayCard(cardUI.Data);

                cardUI.SetParent(fieldRoot);
            });

            cards.Add(cardUI);
        }
    }

    void RefreshCurrentCards()
    {
        foreach (var card in cards)
        {
            card.RefreshPosition();
        }
    }

    void DisableCurrentCards()
    {
        foreach (var card in cards)
        {
            card.SetOnSelected(() => { });
        }
    }

    void DiscardCurrentCards()
    {
        foreach (var card in cards)
        {
            CardSpawner.Instance.DespawnCard(card);
        }

        cards.Clear();
    }
}
