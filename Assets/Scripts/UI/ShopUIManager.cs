using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameManager;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] RectTransform cardsList, packsList, powerupsList;
    [SerializeField] TextMeshProUGUI dosh;

    List<UICard> cardUIs = new();
    List<UICard> packUIs = new();
    List<UICard> upgradeUIs = new();

    List<CardData> playerCards;
    List<CardPackData> playerPacks;
    List<UpgradeCard> playerUpgrades;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        ShopManager.Instance.StateChange += OnStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        ShopManager.Instance.StateChange -= OnStateChanged;
    }

    void OnGameStateChanged(GameManager.GameState gameState)
    {
        var on = gameState == GameManager.GameState.Shop;

        if (on)
            StartCoroutine(DisplayShop());
        else
            transform.localScale = Vector3.zero;
    }

    IEnumerator DisplayShop()
    {
        yield return new WaitForSeconds(2);

        transform.localScale = Vector3.one;

        StartShop();
    }

    void OnStateChanged(ShopManager.ShopStates state, List<CardData> cards, List<CardPackData> packs, List<UpgradeCard> upgrades)
    {
        if (state == ShopManager.ShopStates.PlayerOneBuying)
            PersistentUIManager.Instance.SetPlayer1Turn(true);
        else
            PersistentUIManager.Instance.SetPlayer2Turn(true);

        playerCards = cards;
        playerPacks = packs;
        playerUpgrades = upgrades;

        if(state == ShopManager.ShopStates.PlayerTwoBuying)
        {
            StartShop();
        }
    }

    void StartShop()
    {
        UpdateCardAffordability();

        foreach (var card in playerCards)
        {
            var ui = CardSpawner.Instance.SpawnCard(card, true);
            ui.SetParent(cardsList);

            cardUIs.Add(ui);

            ui.SetOnSelected(() =>
            {
                ShopManager.Instance.BuyItem(card);
                cardUIs.Remove(ui);
                CardSpawner.Instance.DespawnCard(ui);

                RefreshCurrentCards();
                UpdateCardAffordability();
            });
        }

        foreach (var pack in playerPacks)
        {
            var ui = CardSpawner.Instance.SpawnCard(pack, true);
            ui.SetParent(packsList);

            packUIs.Add(ui);

            ui.SetOnSelected(() =>
            {
                ShopManager.Instance.BuyItem(pack);
                packUIs.Remove(ui);
                CardSpawner.Instance.DespawnCard(ui);

                RefreshCurrentCards();
                UpdateCardAffordability();
            });
        }

        foreach (var upgrade in playerUpgrades)
        {
            var ui = CardSpawner.Instance.SpawnCard(upgrade, true);
            ui.SetParent(powerupsList);

            upgradeUIs.Add(ui);

            ui.SetOnSelected(() =>
            {
                ShopManager.Instance.BuyItem(upgrade);
                upgradeUIs.Remove(ui);
                CardSpawner.Instance.DespawnCard(ui);

                RefreshCurrentCards();
                UpdateCardAffordability();
            });
        }
    }

    void UpdateCardAffordability()
    {
        foreach (var card in cardUIs)
        {
            card.SetInteractable(ShopManager.Instance.CanBuyItem(card.Data));
        }

        foreach (var card in packUIs)
        {
            card.SetInteractable(ShopManager.Instance.CanBuyItem(card.Data));
        }

        foreach (var card in upgradeUIs)
        {
            card.SetInteractable(ShopManager.Instance.CanBuyItem(card.Data));
        }

        dosh.SetText($"Dosh: £{ShopManager.Instance.Currency}");
    }

    void RefreshCurrentCards()
    {
        foreach (var card in cardUIs)
        {
            card.RefreshPosition();
        }

        foreach (var card in packUIs)
        {
            card.RefreshPosition();
        }

        foreach (var card in upgradeUIs)
        {
            card.RefreshPosition();
        }
    }

    void DiscardCurrentCards()
    {
        foreach (var card in cardUIs)
        {
            CardSpawner.Instance.DespawnCard(card);
        }
        cardUIs.Clear();

        foreach (var card in packUIs)
        {
            CardSpawner.Instance.DespawnCard(card);
        }
        packUIs.Clear();

        foreach (var card in upgradeUIs)
        {
            CardSpawner.Instance.DespawnCard(card);
        }
        upgradeUIs.Clear();
    }
    
    public void ContinueShopping()
    {
        DiscardCurrentCards();
        ShopManager.Instance.LeaveShop();
    }
}
