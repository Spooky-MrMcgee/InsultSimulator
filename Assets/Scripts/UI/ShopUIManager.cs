using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] RectTransform cardsList, packsList, powerupsList;

    private void Start()
    {
    }

    private void OnDestroy()
    {
        GameManager.gameManager.OnGameStateChanged -= OnStateChanged;
    }

    void OnStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Shop)
            StartShop();
    }

    void StartShop()
    {

    }
}
