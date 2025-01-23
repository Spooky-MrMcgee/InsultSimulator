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

	public GameObject bob, bubba;

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
		SetPlayers();
	}

    private void Start()
	{
		StartCoroutine(StartGame());
    }

	IEnumerator StartGame()
    {
		yield return new WaitForSeconds(0.5f);
		ChangeGameState(GameState.Shop);
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
		currentState = gameState;
		OnGameStateChanged?.Invoke(gameState);
	}

}
