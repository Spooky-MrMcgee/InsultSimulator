using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	public PlayerStruct playerOne { get; private set; }
	public PlayerStruct playerTwo { get; private set; }

	public GameObject bob, bubba;

	[SerializeField] CardLibrary cardLibrary;
	List<CardData> startingCards = new List<CardData>();

    public event Action<GameState> OnGameStateChanged;

	public enum GameState
	{
		Insult,
		Shop,
		Finish
	}

	GameState currentState;

    private void Awake()
    {
        Instance = this;
		SetPlayers();
	}

    private void Start()
	{
		StartCoroutine(StartGame());
    }

	IEnumerator StartGame()
    {
		yield return new WaitForSeconds(0.5f);
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
		playerOne.SetMesh(bubba.gameObject.GetComponentInChildren<SkinnedMeshRenderer>());
		playerTwo.SetMesh(bob.gameObject.GetComponentInChildren<SkinnedMeshRenderer>());
	}

    public void ChangeGameState(GameState gameState)
	{
		if(playerOne.RoundsWon >= 3 || playerTwo.RoundsWon >= 3)
        {
			gameState = GameState.Finish;
        }

		currentState = gameState;
		OnGameStateChanged?.Invoke(gameState);
	}

	public PlayerStruct WinningPlayer => playerOne.RoundsWon > playerTwo.RoundsWon ? playerOne : playerTwo;
	public PlayerStruct LosingPlayer => playerOne.RoundsWon < playerTwo.RoundsWon ? playerOne : playerTwo;
}
