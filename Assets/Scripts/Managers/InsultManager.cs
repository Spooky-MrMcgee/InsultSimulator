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

    public enum PlayerState
    {
        PlayerOne,
        PlayerTwo,
    }

    public enum CardState
    {
        SelectSubject,
        SelectPredicate,
        SelectCompliment,
    }

    public int roundsToPlay = 3;
    public int maxHandSize = 5;

    public event Action<CardState> StateChanged;
    public event Action<PlayerState> PlayerChanged;
    public event Action<List<CardData>> CardsPlayed;
    public event Action Insulted;
    public event Action<int> Scoring;

    public PlayerState currentPlayerState;
    public CardState currentState;

    List<CardData> currentHand = new List<CardData>();
    CardData selectedCard;

    int playedRounds;

    public static InsultManager insultManager;
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += StartInsult;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChanged -= StartInsult;
    }

    private void StartInsult(GameManager.GameState state)
    {
        if (state != GameManager.GameState.Insult) return;

        SetPlayerState(PlayerState.PlayerOne);
        SetState(CardState.SelectSubject);
    }

    void SetPlayerState(PlayerState state)
    {
        currentPlayerState = state;
        PlayerChanged?.Invoke(state);
    }

    void SetState(CardState state)
    {
        currentState = state;
        StateChanged?.Invoke(state);
    }

    public List<CardData> DrawCards()
    {
        return currentState switch
        {
            CardState.SelectPredicate => DrawCards(GetActivePlayer().predicateCards),
            CardState.SelectCompliment => DrawCards(GetActivePlayer().complimentCards),
            _ => DrawCards(GetActivePlayer().subjectCards),
        };
    }

    public List<CardData> DrawCards<T>(List<T> cards) where T : CardData
    {
        int drawCardNumber;
        List<CardData> cardsToDraw = new();

        for (int x = 0; x < maxHandSize;)
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
            x++;
        }

        return cardsToDraw;
    }

    public void SelectCard(CardData card)
    {
        selectedCard = card;
    }

    public void PlayCard()
    {
        if (!selectedCard)
            return;
        else
        {
            currentHand.Add(selectedCard);

            if (currentState == CardState.SelectCompliment)
                FinishRound();
            else
                SetState(currentState + 1);
        }
    }

    void FinishRound()
    {
        CalculateHandScore();
        CardsPlayed?.Invoke(currentHand);

        currentHand.Clear();

        if(currentPlayerState == PlayerState.PlayerTwo)
            playedRounds++;

        if(playedRounds >= roundsToPlay)
        {
            GameManager.Instance.ChangeGameState(GameManager.GameState.Shop);
            playedRounds = 0;
        }
        else
        {
            SetPlayerState(currentPlayerState == PlayerState.PlayerOne ? PlayerState.PlayerTwo : PlayerState.PlayerOne);
            SetState(CardState.SelectSubject);
        }
    }

    void CalculateHandScore()
    {
        int subjectScore = 0;
        int complimentMultiplier = 0;
        int scoreToAdd = 0;
        SubjectCardData subjectCard = null;
        ComplimentCardData complimentCard = null;
        foreach (CardData card in currentHand)
        {
            if (card is SubjectCardData subject)
            {
                subjectScore = subject.score;
                subjectCard = subject;
            }
            else if (card is PredicateCardData predicate)
            { }
            else if (card is ComplimentCardData compliment)
            {
                complimentMultiplier = compliment.score;
                complimentCard = compliment;
            }
        }
        Insulted?.Invoke();

        scoreToAdd = subjectScore * complimentMultiplier;

        if (currentPlayerState == PlayerState.PlayerOne)
        {
            foreach (UpgradeCard upgradeCard in GameManager.Instance.playerOne.upgradeCards)
            {
                scoreToAdd = upgradeCard.OnUpgrade(scoreToAdd, subjectCard);
            }
        }
        else if (currentPlayerState == PlayerState.PlayerTwo)
        {
            foreach (UpgradeCard upgradeCard in GameManager.Instance.playerTwo.upgradeCards)
            {
                scoreToAdd = upgradeCard.OnUpgrade(scoreToAdd, subjectCard);
            }
        }

        GetActivePlayer().IncreaseScore(scoreToAdd);
    }

    PlayerStruct GetActivePlayer()
    {
        return currentPlayerState switch
        {
            PlayerState.PlayerOne => GameManager.Instance.playerOne,
            PlayerState.PlayerTwo => GameManager.Instance.playerTwo,
        };
    }
}
