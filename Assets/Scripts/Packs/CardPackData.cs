using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Pack")]
public class CardPackData : ScriptableObject
{
    public string packName;
    public CardData[] cards;
}
