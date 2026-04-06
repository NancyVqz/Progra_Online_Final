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
    [SerializeField] TextMeshProUGUI lossesText;

    public bool canShoot { get; private set; }

    [Networked] public int player1 { get; set; }
    [Networked] public int player2 { get; set; }
    [Networked] public int player1Victories { get; set; }
    [Networked] public int player2Victories { get; set; }
    [Networked] public int player1Losses { get; set; }
    [Networked] public int player2Losses { get; set; }
    [Networked] private int winningScore { get; set; }
    [Networked] public string player1Username { get; set; }
    [Networked] public string player2Username { get; set; }

    [Networked] public bool barriers { get; set; }
    [SerializeField] public GameObject barrier;
    [SerializeField] public GameObject waitingForPlayersCanvas;

    private void Awake()
    {
        instance = this;
    }

    public override void Spawned()
    {
        ScoreToWin();
        canShoot = true;
        barriers = true;
    }

    public override void Render()
    {
        UpdateUI();

        barrier.SetActive(barriers);
        waitingForPlayersCanvas.SetActive(barriers);
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
        Debug.Log($"Score {player1Username} : {player1} / {player2Username} : {player2}");

        if (player1 == winningScore)
        {
            player1Victories++;
            player2Losses++;
            Rpc_VictoryScreen(player1Username);
        }

        if (player2 == winningScore)
        {
            player2Victories++;
            player1Losses++;
            Rpc_VictoryScreen(player2Username);
        }
    }

    private void UpdateUI()
    {
        player1Score.text = player1Username + " : " + player1;
        player2Score.text = player2Username + " : " + player2;

        scoreToWin.text = "Points to win:" + winningScore;
        victoriesText.text = $"{player1Username} Wins: {player1Victories} \n{player2Username} Wins: {player2Victories}";
        lossesText.text = $"{player1Username} Losses: {player1Losses} \n{player2Username} Losses: {player2Losses}";
    }

    private void ScoreToWin()
    {
        winningScore = Random.Range(10, 25);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void Rpc_VictoryScreen(string winner)
    {
        winnerText.text = "Victory to: " + winner;

        PlayerData playerData = FindFirstObjectByType<PlayerData>();
        int myVictories;
        int myLosses;

        if (Runner.LocalPlayer.PlayerId == 1)
        {
            myVictories = player1Victories;
            myLosses = player1Losses;
        }
        else
        {
            myVictories = player2Victories;
            myLosses = player2Losses;
        }

        if (winner == PlayFabManager._PlayfabManager.username)
        {
            playerData.UpdateClassData("Wins", myVictories.ToString());
        }
        else
        {
            playerData.UpdateClassData("Losses", myLosses.ToString());
        }
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

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void Rpc_CanPlay()
    {
        barrier.SetActive(false);
        waitingForPlayersCanvas.SetActive(false);
    }
}
