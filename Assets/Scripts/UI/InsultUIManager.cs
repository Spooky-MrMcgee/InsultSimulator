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

    InsultManager.PlayerState currentPlayer;

    CardData currentCard;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameStateChanged;
        InsultManager.Instance.PlayerChanged += OnPlayerChanged;
        InsultManager.Instance.StateChanged += OnInsultStateChanged;
        InsultManager.Instance.CardsPlayed += OnCardsPlayed;
        InsultManager.Instance.FinishedRound += OnScoreChanged;
    }

    private void OnDestroy()
    {
        InsultManager.Instance.PlayerChanged -= OnPlayerChanged;
        InsultManager.Instance.StateChanged -= OnInsultStateChanged;
        InsultManager.Instance.CardsPlayed -= OnCardsPlayed;
        InsultManager.Instance.FinishedRound -= OnScoreChanged;
    }

    void GameStateChanged(GameManager.GameState state)
    {
        var on = state == GameManager.GameState.Insult;

        if (on)
            transform.localScale = Vector3.one;
        else
            transform.localScale = Vector3.zero;
    }

    void OnPlayerChanged(InsultManager.PlayerState player)
    {
        currentPlayer = player;

        OnScoreChanged();
    }

    void OnScoreChanged()
    {
        switch (currentPlayer)
        {
            case InsultManager.PlayerState.PlayerOne:
                PersistentUIManager.Instance.SetPlayer1Turn(true);
                break;
            case InsultManager.PlayerState.PlayerTwo:
                PersistentUIManager.Instance.SetPlayer2Turn(true);
                break;
        }
    }

    void OnInsultStateChanged(InsultManager.CardState state)
    {
        StartCoroutine(StartRound(state));
    }

    IEnumerator StartRound(InsultManager.CardState state)
    {
        selectButton.interactable = false;

        //Reset bubbles
        if (state == InsultManager.CardState.SelectSubject)
        {
            yield return new WaitForSeconds(1);
            player1Bubble.ResetText();
            player2Bubble.ResetText();

            bool isPlayer1Turn = InsultManager.Instance.currentPlayerState == InsultManager.PlayerState.PlayerOne;

            player1Bubble.ToggleVisibility(isPlayer1Turn);
            player2Bubble.ToggleVisibility(!isPlayer1Turn);
        }

        //Display cards
        var cards = InsultManager.Instance.DrawCards();

        foreach (var card in cards)
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

    void OnCardsPlayed(List<CardData> cards, string suffix)
    {
        if (suffix != "")
            GetSpeechBubble().AddTo("," + "\n" + suffix);
        Debug.Log(suffix);
        GetSpeechBubble().SetFinished();
    }

    SpeechBubble GetSpeechBubble()
    {
        return currentPlayer == InsultManager.PlayerState.PlayerOne ? player1Bubble : player2Bubble;
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
