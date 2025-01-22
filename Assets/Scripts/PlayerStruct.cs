using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStruct
{
    // Leave room for player upgrades
    public int Score { get; private set; }
    public int RoundsWon { get; private set; }
    List<PredicateCardData> predicateCards = new List<PredicateCardData>();
    List<SubjectCardData> subjectCards = new List<SubjectCardData>();
    List<ComplimentCardData> complimentCards = new List<ComplimentCardData>();

    public void AddCards(CardData card)
    {
        if (card is PredicateCardData predicateCard)
            predicateCards.Add(predicateCard);
        else if (card is SubjectCardData subjectCard)
            subjectCards.Add(subjectCard);
        else if (card is  ComplimentCardData complimentCard)
            complimentCards.Add(complimentCard);
    }

    public void IncreaseScore(int addScore)
    {
        Score += addScore;
    }
    
    public void IncreaseRound()
    {
        RoundsWon++;
    }

    public void AddUpgrade()
    {
        
    }
    
}
