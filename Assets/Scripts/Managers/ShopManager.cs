using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
	public static ShopManager Instance { get; private set; }

    private void Awake()
    {
		Instance = this;
    }

	[SerializeField] int moneyPerRound = 15;

	public int Currency { get; private set; }
	int maxCardLimit = 5, maxPackLimit = 3, maxUpgradeLimit, shopItemToDisplay, randomCardDraw;
	CardData cardToPurchase;
	PlayerStruct playerPurchasing;
	List<CardData> packCards = new List<CardData>();
	CardPackData cardPack;
	[SerializeField] CardLibrary cardLibrary;
	[SerializeField] CardPackLibrary packLibrary;
	
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

	public event Action<ShopStates, List<CardData>, List<CardPackData>> StateChange;
	public event Action<List<CardData>> PackOpened;

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
			ChangeShopState(ShopStates.PlayerOneBuying);
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
		cardsToDisplay.Clear();

        for (int x = 0; x < maxCardLimit; x++)
        {
            shopItemToDisplay = UnityEngine.Random.Range(0, cardLibrary.cards.Count);
            cardsToDisplay.Add(cardLibrary.cards[shopItemToDisplay]);
        }
    }

	void PopulatePacks()
	{
		packsToDisplay.Clear();

        for (int x = 0; x < maxPackLimit; x++)
        {
            shopItemToDisplay = UnityEngine.Random.Range(0, packLibrary.cardPacks.Count);
            packsToDisplay.Add(packLibrary.cardPacks[shopItemToDisplay]);
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
		if (purchasedItem is CardPackData packItem && Currency >= packItem.cost)
		{
			OpenPack(packItem);
			Currency -= packItem.cost;
		}
		else if (purchasedItem is CardData cardItem && Currency >= cardItem.cost)
		{
			playerPurchasing.AddCards(cardItem);
			Currency -= cardItem.cost;
		}
		else if (purchasedItem is UpgradeCard upgradeItem && Currency >= upgradeItem.cost)
		{
			playerPurchasing.AddUpgrade(upgradeItem);
			Currency -= upgradeItem.cost;
		}
	}

	public bool CanBuyItem<T>(T item)
    {
		if (item is CardPackData packItem)
		{
			return Currency >= packItem.cost;
		}
		else if (item is CardData cardItem)
		{
			return Currency >= cardItem.cost;
		}
		else if (item is UpgradeCard upgradeItem)
		{
			return Currency >= upgradeItem.cost;
		}

		return false;
	}

	void OpenPack(CardPackData cardPack)
	{
		for (int x = 0; x < cardPack.cardsToDraw; x++)
		{
			randomCardDraw = UnityEngine.Random.Range(0, cardPack.cards.Length);
			packCards.Add(cardPack.cards[randomCardDraw]);
			playerPurchasing.AddCards(cardPack.cards[randomCardDraw]);
		}
		PackOpened?.Invoke(packCards);
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
	
	PlayerStruct GetCurrentPlayer()
	{
		PlayerStruct currentPlayer = null;
		if (shopStates == ShopStates.PlayerOneBuying)
			currentPlayer = GameManager.gameManager.playerOne;
		else if (shopStates == ShopStates.PlayerTwoBuying)
			currentPlayer = GameManager.gameManager.playerTwo;
		return currentPlayer;
	}

	void ChangeShopState(ShopStates state)
	{
		Shop();
		Currency = moneyPerRound;
		StateChange?.Invoke(state, cardsToDisplay, packsToDisplay);
	}
}
