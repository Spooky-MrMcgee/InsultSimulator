using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStruct
{
    // Leave room for player upgrades
    public int Score { get; private set; }
    public int RoundsWon { get; private set; }
    public int maxHandSize { get; private set; } = 5;
    public SkinnedMeshRenderer characterMesh { get; set; }
    public List<PredicateCardData> predicateCards { get; private set; } = new List<PredicateCardData>();
    public List<SubjectCardData> subjectCards { get; private set; } = new List<SubjectCardData>();
    public List<ComplimentCardData> complimentCards { get; private set; } = new List<ComplimentCardData>();
    public List<UpgradeCard> upgradeCards { get; private set; }

    public void AddCards(CardData card)
    {
        if (card is PredicateCardData predicateCard)
            predicateCards.Add(predicateCard);
        else if (card is SubjectCardData subjectCard)
            subjectCards.Add(subjectCard);
        else if (card is  ComplimentCardData complimentCard)
            complimentCards.Add(complimentCard);
    }

    public void AddUpgrade(UpgradeCard upgradeCard)
    { 
        upgradeCards.Add(upgradeCard); 
    }

    public void SetMesh(SkinnedMeshRenderer meshRenderer)
    {
        characterMesh = meshRenderer;
    }

    public void IncreaseScore(int addScore)
    {
        Score += addScore;
    }
    
    public void IncreaseRound()
    {
        RoundsWon++;
    }
}
