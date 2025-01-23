using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsultManager : MonoBehaviour
{
    public static InsultManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public enum PlayerSelection
    {
        PlayerOne,
        PlayerTwo,
    }

    public enum CardStates
    {
        SelectSubject,
        SelectPredicate,
        SelectCompliment,
        PlayCards,
    }

    public event Action<CardStates, List<CardData>> StateChanged;
    public event Action<PlayerSelection, PlayerStruct> PlayerSelected;
    public event Action<List<CardData>> CardsPlayed;
    public event Action Insulted;
    public PlayerSelection currentPlayerState { get; private set; }
    CardStates currentState;
    List<CardData> cardsToDraw = new List<CardData>();
    List<CardData> currentHand = new List<CardData>();
    CardData selectedCard;
    PlayerStruct currentPlayer;
    int drawCardNumber;
    int subjectScore, complimentMultiplier;
    int playedRounds;
    public static InsultManager insultManager;
    private void Start()
    {
        GameManager.gameManager.OnGameStateChanged += StartInsult;
    }

    private void OnDestroy()
    {
        GameManager.gameManager.OnGameStateChanged -= StartInsult;
    }

    private void StartInsult(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.Insult)
        {
            currentPlayerState = PlayerSelection.PlayerOne;
            ChangePlayerState(currentPlayerState);
            ChangeCardStates(CardStates.SelectSubject);
            Debug.Log("Insult is starting");
        }
    }

    private void SelectCardType(PlayerStruct player)
    {
        switch (currentState)
        {
            case CardStates.SelectSubject:
                Debug.Log("Starting Subject Selection");
                DrawCards(currentPlayer.subjectCards);
                break;

            case CardStates.SelectPredicate:
                DrawCards(currentPlayer.predicateCards);
                Debug.Log("Starting Predicate Selection");
                break;

            case CardStates.SelectCompliment:
                DrawCards(currentPlayer.complimentCards);
                Debug.Log("Starting Compliment Selection");
                break;

            case CardStates.PlayCards:
                PlayAllCards(currentHand);
                Debug.Log("Playing Hand.");
                break;
        }

    }

    private void DrawCards<T>(List<T> cards) where T : CardData
    {
        cardsToDraw.Clear();
        for (int x = 0; x < currentPlayer.maxHandSize;)
        {
            bool foundCard = false;
            drawCardNumber = UnityEngine.Random.Range(0, cards.Count);
            foreach (CardData pulledCards in cardsToDraw)
            {
                if (cards[drawCardNumber] == pulledCards)
                    foundCard = true;
            }
            if (foundCard)
                continue;
            cardsToDraw.Add(cards[drawCardNumber]);
            Debug.Log(cardsToDraw[x].content);
            x++;
        }
    }

    public void ChangePlayerState(PlayerSelection player)
    {
        if (player == PlayerSelection.PlayerOne)
            currentPlayer = GameManager.gameManager.playerOne;
        else if (player == PlayerSelection.PlayerTwo)
            currentPlayer = GameManager.gameManager.playerTwo;

        currentPlayerState = player;
        PlayerSelected?.Invoke(player, currentPlayer);
    }

    public void SelectCard(CardData card)
    {
        if (card != selectedCard)
            selectedCard = card;
    }

    public void PlayCard()
    {
        if (!selectedCard)
            return;
        else
        {
            currentHand.Add(selectedCard);
            ChangeCardStates(currentState + 1);
        }
    }

    public void PlayAllCards(List<CardData> cardsToPlay)
    {
        foreach (CardData card in cardsToPlay)
        {
            if (card is SubjectCardData subject)
                subjectScore = subject.score;
            else if (card is PredicateCardData predicate)
            { }
            else if (card is ComplimentCardData compliment)
                complimentMultiplier = compliment.score;
        }
        Insulted?.Invoke();
        currentPlayer.IncreaseScore(subjectScore * complimentMultiplier);
        ChangeCardStates(CardStates.SelectSubject);
    }

    public void ChangeCardStates(CardStates state)
    {
        currentState = state;

        if (state == CardStates.PlayCards)
        {
            CardsPlayed?.Invoke(currentHand);
            if (currentPlayerState == PlayerSelection.PlayerOne)
            {
                ChangePlayerState(PlayerSelection.PlayerTwo); 
                SelectCardType(currentPlayer);
                StateChanged?.Invoke(state, cardsToDraw);
            }
            else if (currentPlayerState == PlayerSelection.PlayerTwo)
            {
                playedRounds++;

                if (playedRounds > 3)
                {
                    playedRounds = 0;
                    GameManager.gameManager.ChangeGameState(GameManager.GameState.Shop);
                }
                else
                {
                    ChangePlayerState(PlayerSelection.PlayerOne); 
                    SelectCardType(currentPlayer);
                    StateChanged?.Invoke(state, cardsToDraw);
                }
            }
        }
        else
        {
            SelectCardType(currentPlayer);
            StateChanged?.Invoke(state, cardsToDraw);
        }
    }
}
