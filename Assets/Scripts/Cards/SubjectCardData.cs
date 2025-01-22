using UnityEngine;

[CreateAssetMenu(menuName = "Card/Subject")]
public class SubjectCardData : CardData
{
    public int score;
    public int cost;
    public SubjectType type;

    public enum SubjectType
    {
        Family,
        Relationship,
        Style,
        Appearance,
        Character,
        Status
    }
}
