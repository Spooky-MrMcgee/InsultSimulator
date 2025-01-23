using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Upgrade")]
public class UpgradeCard : ScriptableObject
{ 
    public string name;
    public int cost;
    public UpgradeEffect effect;
}
