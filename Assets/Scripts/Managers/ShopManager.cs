using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
	int currency, maxCardLimit = 5, maxPackLimit = 3, maxUpgradeLimit, shopItemToDisplay, randomCardDraw;
	CardData cardToPurchase;
	PlayerStruct playerPurchasing;
	List<CardData> packCards = new List<CardData>();
	CardPackData cardPack;
	[SerializeField] CardLibrary cardLibrary;
	
	[SerializeField] List<CardData> cardsToDisplay = new List<CardData>();
	[SerializeField] List<CardPackData> packsToDisplay = new List<CardPackData>();
	// List<Packs> packs;
	// List<Upgrades> upgrades;
	public enum ShopStates
	{
		PlayerOneBuying,
		PlayerTwoBuying,
	}

	ShopStates shopStates;

	public event Action<ShopStates> StateChange;

    private void Start()
    {
		GameManager.gameManager.OnGameStateChanged += StartShop;
    }

    private void OnDestroy()
    {
        GameManager.gameManager.OnGameStateChanged -= StartShop;
    }
    public void StartShop(GameManager.GameState state)
	{
		if (state == GameManager.GameState.Shop)
		{
			Shop();
			shopStates = ShopStates.PlayerOneBuying;
		}
	}

	void Shop()
	{
		PopulateCards();
		PopulatePacks();
		PopulateUpgrades();
	}

	void PopulateCards()
	{
        for (int x = 0; x < maxCardLimit; x++)
        {
            shopItemToDisplay = UnityEngine.Random.Range(0, cardLibrary.cards.Count);
            cardsToDisplay.Add(cardLibrary.cards[shopItemToDisplay]);
        }
    }

	void PopulatePacks()
	{
        for (int x = 0; x < maxPackLimit; x++)
        {
            shopItemToDisplay = UnityEngine.Random.Range(0, cardLibrary.cards.Count);
            cardsToDisplay.Add(cardLibrary.cards[shopItemToDisplay]);
        }
    }
	void PopulateUpgrades()
	{
        for (int x = 0; x < maxUpgradeLimit; x++)
        {
            shopItemToDisplay = UnityEngine.Random.Range(0, cardLibrary.cards.Count);
            cardsToDisplay.Add(cardLibrary.cards[shopItemToDisplay]);
        }
    }

	public void BuyItem<T>(T purchasedItem)
	{
		playerPurchasing = GetCurrentPlayer();
		if (purchasedItem is CardPackData packItem)
		{
			OpenPack(packItem);
		}
		else if (purchasedItem is CardData cardItem)
		{
			playerPurchasing.AddCards(cardItem);
		}
		// else if (purchasedItem is Upgrade)
	}

	public void OpenPack(CardPackData cardPack)
	{
		for (int x = 0; x < cardPack.cardsToDraw; x++)
		{
			UnityEngine.Random.Range(0, cardPack.cards.Length);
		}
	}
	void LeaveShop()
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
	
	PlayerStruct GetCurrentPlayer()
	{
		PlayerStruct currentPlayer = null;
		if (shopStates == ShopStates.PlayerOneBuying)
			currentPlayer = GameManager.gameManager.playerOne;
		else if (shopStates == ShopStates.PlayerTwoBuying)
			currentPlayer = GameManager.gameManager.playerTwo;
		return currentPlayer;
	}

	public void ChangeShopState(ShopStates state)
	{
		StateChange?.Invoke(state);
		Shop();
	}
}
