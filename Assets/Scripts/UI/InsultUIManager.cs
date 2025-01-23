using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsultUIManager : MonoBehaviour
{
    [SerializeField] RectTransform handRoot;
    [SerializeField] Button selectButton;

    List<UICard> hand = new();

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
        selectButton.interactable = false;

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
                selectButton.interactable = true;
            });

            hand.Add(cardUI);
        }
    }

    public void PlayCard()
    {
        CardManager.Instance.PlayCard();
    }

    void RefreshCurrentCards()
    {
        foreach (var card in hand)
        {
            card.RefreshPosition();
        }
    }

    void DisableCurrentCards()
    {
        foreach (var card in hand)
        {
            card.SetOnSelected(() => { });
        }
    }

    void DiscardCurrentCards()
    {
        foreach (var card in hand)
        {
            CardSpawner.Instance.DespawnCard(card);
        }

        hand.Clear();
    }
}
