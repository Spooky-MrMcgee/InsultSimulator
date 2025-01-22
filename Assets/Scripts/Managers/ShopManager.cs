using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
	int currency, maxCardLimit = 5, cardToAdd;
	[SerializeField] CardLibrary cardLibrary;
	
	[SerializeField] List<CardData> cardsToDisplay = new List<CardData>();
	// List<Packs> packs;
	// List<Upgrades> upgrades;
	public enum ShopStates
	{
		PlayerOneBuying,
		PlayerTwoBuying,
	}

	ShopStates shopStates;

	public event Action<ShopStates> StateChange;

    private void OnEnable()
    {
		GameManager.gameManager.OnGameStateChanged += StartShop;
    }

    private void OnDisable()
    {
        GameManager.gameManager.OnGameStateChanged -= StartShop;
    }
    public void StartShop(GameManager.GameState state)
	{
		if (state == GameManager.GameState.Shop)
		{
			Shop();
		}
	}

	public void Shop()
	{
        currency = 15;
        for (int x = 0; x < maxCardLimit; x++)
		{ 
			cardToAdd = UnityEngine.Random.Range(0, cardLibrary.cards.Count);
			cardsToDisplay.Add(cardLibrary.cards[cardToAdd]);
		}
	}

	public void LeaveShop()
	{
		if (shopStates == ShopStates.PlayerOneBuying)
		{
			ChangeShopState(ShopStates.PlayerTwoBuying);
		}
		else if (shopStates == ShopStates.PlayerTwoBuying)
		{
			GameManager.gameManager.ChangeGameState(GameManager.GameState.Insult);
		}
	}
	
	public void ChangeShopState(ShopStates state)
	{
		StateChange?.Invoke(state);
		Shop();
	}
}
