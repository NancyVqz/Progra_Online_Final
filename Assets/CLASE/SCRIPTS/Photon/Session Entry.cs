using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fusion;

public class SessionEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text sessionName;
    [SerializeField] private TMP_Text playerCount;
    [SerializeField] private Button joinButton;

    private GameObject lobbyCanvas;

    private void Start()
    {
        lobbyCanvas = GameObject.Find("LobbiesPanel");
    }

    public void SetInfo(SessionInfo sessionInfo)
    {
        sessionName.text = sessionInfo.Name;
        playerCount.text = sessionInfo.PlayerCount.ToString()
            + "/" +
            sessionInfo.MaxPlayers.ToString();

        if(sessionInfo.PlayerCount >= sessionInfo.MaxPlayers)
        {
            joinButton.interactable = false;
        }
    }

    public void JoinSession() // Para unirte a un lobby
    {
        PhotonManager._PhotonManager.JoinSession(sessionName.text);
        lobbyCanvas.SetActive(false);
    }

}
