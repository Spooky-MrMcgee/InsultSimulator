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
        InsultManager.Instance.PlayerSelected += OnPlayerChanged;
        InsultManager.Instance.StateChanged += OnInsultStateChanged;
    }

    private void OnDestroy()
    {
        InsultManager.Instance.PlayerSelected -= OnPlayerChanged;
        InsultManager.Instance.StateChanged -= OnInsultStateChanged;
    }

    void OnPlayerChanged(InsultManager.PlayerSelection player, PlayerStruct data)
    {
        switch(player)
        {
            case InsultManager.PlayerSelection.PlayerOne:
                PersistentUIManager.Instance.SetPlayer1Turn(true);
                break;
            case InsultManager.PlayerSelection.PlayerTwo:
                PersistentUIManager.Instance.SetPlayer2Turn(true);
                break;
        }
    }

    void OnInsultStateChanged(InsultManager.CardStates state, List<CardData> availableCards)
    {
        Debug.Log(availableCards.Count);

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

                InsultManager.Instance.SelectCard(cardUI.Data);
                selectButton.interactable = true;
            });

            hand.Add(cardUI);
        }
    }

    public void PlayCard()
    {
        InsultManager.Instance.PlayCard();
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
