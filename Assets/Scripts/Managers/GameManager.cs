using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager gameManager { get; private set; }

    private void Awake()
    {
		gameManager = this;
    }

    public event Action<GameState> OnGameStateChanged;

	public enum GameState
	{
		Insult,
		Shop,
	}

	GameState currentState;

	private void Start()
	{
		currentState = GameState.Insult;
	}

    public void ChangeGameState(GameState gameState)
	{
		OnGameStateChanged?.Invoke(gameState);
	}

}
