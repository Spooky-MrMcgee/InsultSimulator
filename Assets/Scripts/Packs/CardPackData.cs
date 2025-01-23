using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Pack")]
public class CardPackData : CardData
{
    public int cardsToDraw;
    public CardData[] cards;
}
