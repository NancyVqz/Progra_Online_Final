using System.Collections;
using Fusion;
using TMPro;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public static ScoreManager instance;

    [SerializeField] TextMeshProUGUI player1Score;
    [SerializeField] TextMeshProUGUI player2Score;
    [SerializeField] TextMeshProUGUI scoreToWin;

    [SerializeField] GameObject victoryCanvas;
    [SerializeField] TextMeshProUGUI winnerText;

    [SerializeField] TextMeshProUGUI victoriesText;

    public bool canShoot { get; private set; }

    [Networked] public int player1 { get; set; }
    [Networked] public int player2 { get; set; }
    [Networked] public int player1Victories { get; set; }
    [Networked] public int player2Victories { get; set; }
    [Networked] private int winningScore { get; set; }

    private void Awake()
    {
        instance = this;
    }

    public override void Spawned()
    {
        ScoreToWin();
        canShoot = true;
    }

    public override void Render()
    {
        UpdateUI();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_UpdateScore(PlayerRef shooter)
    {
        if (shooter.PlayerId == 1)
        {
            player1++;
        }
        else if (shooter.PlayerId == 2)
        {
            player2++;
        }
        Debug.Log($"Score P1: {player1} / P2: {player2}");

        if (player1 == winningScore)
        {
            player1Victories++;
            Rpc_VictoryScreen(1);
        }

        if (player2 == winningScore)
        {
            player2Victories++;
            Rpc_VictoryScreen(2);
        }
    }

    private void UpdateUI()
    {
        player1Score.text = "Player 1: " + player1;
        player2Score.text = "Player 2: " + player2;
        scoreToWin.text = "Points to win:" + winningScore;
        victoriesText.text = $"Player 1 Wins: {player1Victories} \nPlayer 2 Wins: {player2Victories}";
    }

    private void ScoreToWin()
    {
        winningScore = Random.Range(10, 25);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void Rpc_VictoryScreen(int winner)
    {
        winnerText.text = "Victory to: Player " + winner;
        StartCoroutine(ShowVictory());
    }

    private IEnumerator ShowVictory()
    {
        canShoot = false;
        victoryCanvas.SetActive(true);

        yield return new WaitForSeconds(4);

        victoryCanvas.SetActive(false);
        player1 = 0;
        player2 = 0;
        ScoreToWin();
        canShoot = true;
    }
}
