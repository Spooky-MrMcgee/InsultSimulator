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
        InsultManager.Instance.StateChanged += OnInsultStateChanged;
    }

    private void OnDestroy()
    {
        InsultManager.Instance.StateChanged -= OnInsultStateChanged;
    }

    void OnPlayerChanged(InsultManager.PlayerSelection player)
    {
    }

    void OnInsultStateChanged(InsultManager.CardStates state, List<CardData> availableCards)
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

                InsultManager.Instance.SelectCard(cardUI.Data);
                InsultManager.Instance.PlayCard(cardUI.Data);

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
