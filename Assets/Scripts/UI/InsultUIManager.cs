using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsultUIManager : MonoBehaviour
{
    [SerializeField] RectTransform handRoot;
    [SerializeField] Button selectButton;

    [SerializeField] SpeechBubble player1Bubble, player2Bubble;

    List<UICard> hand = new();

    InsultManager.PlayerSelection currentPlayer;

    CardData currentCard;

    private void Start()
    {
        InsultManager.Instance.PlayerSelected += OnPlayerChanged;
        InsultManager.Instance.StateChanged += OnInsultStateChanged;
        InsultManager.Instance.CardsPlayed += OnCardsPlayed;
    }

    private void OnDestroy()
    {
        InsultManager.Instance.PlayerSelected -= OnPlayerChanged;
        InsultManager.Instance.StateChanged -= OnInsultStateChanged;
        InsultManager.Instance.CardsPlayed -= OnCardsPlayed;
    }

    void OnPlayerChanged(InsultManager.PlayerSelection player, PlayerStruct data)
    {
        currentPlayer = player;

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
        StartCoroutine(StartRound(state, availableCards));
    }

    IEnumerator StartRound(InsultManager.CardStates state, List<CardData> availableCards)
    {
        selectButton.interactable = false;

        if (state != InsultManager.CardStates.PlayCards)
        {
            if (state == InsultManager.CardStates.SelectSubject)
            {
                yield return new WaitForSeconds(1);
                player1Bubble.ResetText();
                player2Bubble.ResetText();

                bool isPlayer1Turn = InsultManager.Instance.currentPlayerState == InsultManager.PlayerSelection.PlayerOne;

                player1Bubble.ToggleVisibility(isPlayer1Turn);
                player2Bubble.ToggleVisibility(!isPlayer1Turn);
            }

            foreach (var card in availableCards)
            {
                var cardUI = CardSpawner.Instance.SpawnCard(card, false);
                cardUI.SetParent(handRoot);

                cardUI.SetOnSelected(() =>
                {
                    InsultManager.Instance.SelectCard(card);
                    selectButton.interactable = true;

                    if (currentCard != null)
                    {
                        GetSpeechBubble().RemoveFrom(currentCard.content);
                    }
                    GetSpeechBubble().AddTo(card.content);
                    currentCard = card;
                });

                hand.Add(cardUI);
            }
        }
    }

    void OnCardsPlayed(List<CardData> cards)
    {
        GetSpeechBubble().SetFinished();
    }

    SpeechBubble GetSpeechBubble()
    {
        return currentPlayer == InsultManager.PlayerSelection.PlayerOne ? player1Bubble : player2Bubble;
    }


    public void PlayCard()
    {
        DisableCurrentCards();
        DiscardCurrentCards();
        currentCard = null;
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
