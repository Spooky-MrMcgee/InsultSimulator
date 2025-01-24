using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersistentUIManager : MonoBehaviour
{
    public static PersistentUIManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI player1Score, player2Score;
    [SerializeField] RectTransform player1Turn, player2Turn;

    [SerializeField] PlayerRoundCounter player1Rounds, player2Rounds;

    [SerializeField] int player1CurrentScore = 0, player2CurrentScore = 0;

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
        StartCoroutine(IncrementPlayer1Score(score));
    }

    IEnumerator IncrementPlayer1Score(int newScore)
    {
        float timer = 0;

        while(timer < 1)
        {
            player1Score.SetText(((int)Mathf.Lerp(player1CurrentScore, newScore, EaseOutQuint(timer))).ToString());
            timer += 0.02f;
            yield return new WaitForSeconds(0.02f);
        }

        player1CurrentScore = newScore;
        player1Score.SetText(player1CurrentScore.ToString());

        float EaseOutQuint(float time)
        {
            return 1 - Mathf.Pow(1 - time, 5);
        }
    }

    public void SetPlayer1RoundsWon(int rounds)
    {
        player1Rounds.SetScore(rounds);
    }

    public void SetPlayer2RoundsWon(int rounds)
    {
        player2Rounds.SetScore(rounds);
    }

    public void SetPlayer2Score(int score)
    {
        StartCoroutine(IncrementPlayer2Score(score));
    }

    IEnumerator IncrementPlayer2Score(int newScore)
    {
        float timer = 0;

        while (timer < 1)
        {
            player2Score.SetText(((int)Mathf.Lerp(player2CurrentScore, newScore, EaseOutQuint(timer))).ToString());
            timer += 0.02f;
            yield return new WaitForSeconds(0.02f);
        }

        player2CurrentScore = newScore;
        player2Score.SetText(player2CurrentScore.ToString());

        float EaseOutQuint(float time) {
            return 1 - Mathf.Pow(1 - time, 5);
        }
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

        SetPlayer1RoundsWon(GameManager.Instance.playerOne.RoundsWon);
        SetPlayer2RoundsWon(GameManager.Instance.playerTwo.RoundsWon);
    }
}
