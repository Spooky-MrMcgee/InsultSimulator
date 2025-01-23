using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Pack")]
public class CardPackData : ScriptableObject
{
    public string packName;
    public int cardsToDraw;
    public int cost;
    public CardData[] cards;
}
