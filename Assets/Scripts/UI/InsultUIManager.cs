using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsultUIManager : MonoBehaviour
{
    [SerializeField] RectTransform fieldRoot;
    [SerializeField] RectTransform handRoot;

    [SerializeField] List<CardData> startingCards;

    List<UICard> cards = new();

    private void Start()
    {
        OnInsultStateChanged(CardManager.CardStates.SelectSubject, startingCards);
    }

    void OnPlayerChanged(CardManager.PlayerSelection player)
    {

    }

    void OnInsultStateChanged(CardManager.CardStates state, List<CardData> availableCards)
    {
        foreach (var card in availableCards)
        {
            var cardUI = CardSpawner.Instance.SpawnCard(card);
            cardUI.SetParent(handRoot);

            cardUI.SetOnSelected(() =>
            {
                //Give card manager new card
                cardUI.SetParent(fieldRoot);
                cardUI.SetOnSelected(() => { });

                foreach (var card in cards)
                {
                    card.RefreshPosition();
                }
            });

            cards.Add(cardUI);
        }
    }
}
