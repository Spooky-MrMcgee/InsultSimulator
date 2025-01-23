using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	public static CardManager Instance;

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
	PlayerSelection currentPlayerState;
	CardStates currentState;
	List<CardData> cardsToDraw = new List<CardData>();
	List<CardData> currentHand = new List<CardData>();
	CardData selectedCard;
	PlayerStruct currentPlayer;
	int drawCardNumber;
	int subjectScore, complimentMultiplier;

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
		Debug.Log("Drawing Cards");
		for (int x = 0; x < currentPlayer.maxHandSize; x++)
		{
            drawCardNumber = UnityEngine.Random.Range(0, cards.Count);
			cardsToDraw.Add(cards[drawCardNumber]);
			Debug.Log(cardsToDraw[x].content);
        }
	}

	public void ChangePlayerState(PlayerSelection player)
	{
		if (player == PlayerSelection.PlayerOne)
			currentPlayer = GameManager.gameManager.playerOne;
		else if (player == PlayerSelection.PlayerTwo)
			currentPlayer = GameManager.gameManager.playerTwo;

		PlayerSelected?.Invoke(player, currentPlayer);
	}

	public void SelectCard(CardData card)
	{
		if (selectedCard == card)
			selectedCard = null;
		else if (card != selectedCard)
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
		currentPlayer.IncreaseScore(subjectScore * complimentMultiplier);
	}

	public void ChangeCardStates(CardStates state)
	{
		currentState = state;

		SelectCardType(currentPlayer);
		StateChanged?.Invoke(state, cardsToDraw);

		if (state == CardStates.PlayCards)
			CardsPlayed?.Invoke(currentHand);
	}
}
