using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Upgrade")]
public abstract class UpgradeCard : CardData
{
    public string insultAddon;
    public abstract int OnUpgrade(int score, SubjectCardData subjectCard);
}

