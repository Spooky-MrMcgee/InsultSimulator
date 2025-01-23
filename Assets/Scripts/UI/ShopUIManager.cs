using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] RectTransform cardsList, packsList, powerupsList;
    [SerializeField] TextMeshProUGUI dosh;

    List<UICard> cardUIs = new();

    private void Start()
    {
        GameManager.gameManager.OnGameStateChanged += OnGameStateChanged;
        ShopManager.Instance.StateChange += OnStateChanged;
    }

    private void OnDestroy()
    {
        ShopManager.Instance.StateChange -= OnStateChanged;
    }

    void OnGameStateChanged(GameManager.GameState gameState)
    {
        gameObject.SetActive(gameState == GameManager.GameState.Shop);
    }

    void OnStateChanged(ShopManager.ShopStates state, List<CardData> cards, List<CardPackData> packs)
    {
        if (state == ShopManager.ShopStates.PlayerOneBuying)
            PersistentUIManager.Instance.SetPlayer1Turn(true);
        else
            PersistentUIManager.Instance.SetPlayer2Turn(true);

        StartShop(cards, packs);
    }

    void StartShop(List<CardData> cards, List<CardPackData> packs)
    {
        UpdateCardAffordability();

        foreach (var card in cards)
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

        foreach (var pack in packs)
        {
            //var ui = CardSpawner.Instance.SpawnCard(pack);
            //ui.SetParent(cardsList);
        }
    }

    void UpdateCardAffordability()
    {
        foreach (var card in cardUIs)
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
    }

    void DiscardCurrentCards()
    {
        foreach (var card in cardUIs)
        {
            CardSpawner.Instance.DespawnCard(card);
        }

        cardUIs.Clear();
    }
}
