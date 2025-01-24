using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Upgrade/Category")]
public class UpgradeCategory : UpgradeCard
{
    public int multiplier;
    public SubjectCardData.SubjectType subjectType;
    public override int OnUpgrade(int score, SubjectCardData subjectCard)
    {
        if (subjectCard.type == subjectType)
        {
            score *= multiplier;
        }
        return score;
        throw new System.NotImplementedException();
    }
}
