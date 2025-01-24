using TMPro;
using UnityEngine;

public class PersistentUIManager : MonoBehaviour
{
    public static PersistentUIManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI player1Score, player2Score;
    [SerializeField] RectTransform player1Turn, player2Turn;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateScore();
    }

    public void SetPlayer1Score(int score)
    {
        player1Score.SetText(score.ToString());
    }

    public void SetPlayer2Score(int score)
    {
        player2Score.SetText(score.ToString());
    }

    public void SetPlayer1Turn(bool on)
    {
        player1Turn.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, on ? 150 : 30);

        UpdateScore();

        if (on)
            SetPlayer2Turn(false);
    }

    public void SetPlayer2Turn(bool on)
    {
        player2Turn.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, on ? 150 : 30);

        UpdateScore();

        if (on)
            SetPlayer1Turn(false);
    }

    public void UpdateScore()
    {
        SetPlayer1Score(GameManager.Instance.playerOne.Score);
        SetPlayer2Score(GameManager.Instance.playerTwo.Score);
    }
}
