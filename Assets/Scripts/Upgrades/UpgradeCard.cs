using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Upgrade")]
public class UpgradeCard : ScriptableObject
{ 
    public string upgradeName;
    public int cost;
    public UpgradeEffect effect;
}
