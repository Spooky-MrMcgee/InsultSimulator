using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public static CardSpawner Instance { get; private set; }

    [SerializeField] UICard prefab;
    [SerializeField] UICardVisual visualPrefab;

    [SerializeField] RectTransform deck;
    [SerializeField] RectTransform bin;

    [SerializeField] RectTransform visualRoot;

    private void Awake()
    {
        Instance = this;
    }

    public UICard SpawnCard(CardData card, bool showPrice)
    {
        var inst = Instantiate(prefab, deck);
        var visualInst = Instantiate(visualPrefab, visualRoot);

        inst.Setup(card, visualInst, showPrice);

        return inst;
    }

    public void DespawnCard(UICard card)
    {
        card.SetParent(bin);

        Destroy(card.card.gameObject, 1);
        Destroy(card.gameObject, 1);
    }
}
