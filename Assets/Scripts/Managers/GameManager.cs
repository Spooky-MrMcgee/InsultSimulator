using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager gameManager { get; private set; }
	public PlayerStruct playerOne { get; private set; }
	public PlayerStruct playerTwo { get; private set; }

	[SerializeField] CardLibrary cardLibrary;
	List<CardData> startingCards = new List<CardData>();

    public event Action<GameState> OnGameStateChanged;

	public enum GameState
	{
		Insult,
		Shop,
	}

	GameState currentState;

    private void Awake()
    {
        gameManager = this;
    }

    private void Start()
	{
		SetPlayers();
        ChangeGameState(GameState.Insult);
    }

	private void SetPlayers()
	{
		playerOne = new PlayerStruct();
		playerTwo = new PlayerStruct();
		foreach (CardData card in cardLibrary.cards)
		{
            playerOne.AddCards(card);
			playerTwo.AddCards(card);
        }
		Debug.Log(playerOne.subjectCards.ToString());
		Debug.Log(playerOne.predicateCards.ToString());
		Debug.Log(playerOne.complimentCards.ToString());
	}

    public void ChangeGameState(GameState gameState)
	{
		currentState = gameState;
		OnGameStateChanged?.Invoke(gameState);
	}

}
