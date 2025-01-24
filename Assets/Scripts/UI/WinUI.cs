using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title, rounds, score, insult;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void OnGameStateChanged(GameManager.GameState state)
    {
        if(state != GameManager.GameState.Finish)
        {
            transform.localScale = Vector3.zero;
        }
        else
            Invoke(nameof(Show), 2);
    }

    void Show()
    {
        transform.localScale = Vector3.one;
        SetGameOver();
    }

    void SetGameOver()
    {
        var winningPlayer = GameManager.Instance.WinningPlayer;
        var playerTag = winningPlayer == GameManager.Instance.playerOne ? "Player 1" : "Player 2";

        title.SetText($"{playerTag} wins!");
        rounds.SetText($"Rounds won: {GameManager.Instance.WinningPlayer.RoundsWon}/{GameManager.Instance.WinningPlayer.RoundsWon + GameManager.Instance.LosingPlayer.RoundsWon}");
        score.SetText($"Total score: {GameManager.Instance.WinningPlayer.TotalScore} pts");

        var sentence = string.Join(" ", winningPlayer.highestScoreInsult.Select((c) => c.content));
        insult.SetText($"Most damning insult ({winningPlayer.highestScore} pts):\n\"{sentence}!\"");
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
