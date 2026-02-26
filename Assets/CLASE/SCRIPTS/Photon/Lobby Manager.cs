using Fusion;
using TMPro;
using UnityEngine;


// Este script es el que va a actualizar mi lista de sesiones en el canvas
public class LobbyManager : MonoBehaviour
{
    [SerializeField] private Transform viewportContent;
    [SerializeField] private GameObject lobbyPrefab;
    [SerializeField] private GameObject warningMessage;

    [SerializeField] private TMP_Text maxPlayerCountText;
    private int maxPlayerCount = 1;

    public TMP_Text sessionNameCustom;
    public TMP_Text maxPlayersCustom;

    private void OnEnable()
    {
        if (PhotonManager._PhotonManager != null)
        {
            PhotonManager._PhotonManager.onSessionListUpdated += DestroyCanvasContent;
            PhotonManager._PhotonManager.onSessionListUpdated += UpdateSessionCanvas;
        }
        else
        {
            Debug.LogError("No se encontro PhotonManager");
        }
    }

    public void UpdateSessionCanvas()
    {
        Debug.Log("Creando sesiones: " + PhotonManager._PhotonManager.availableSessions.Count);
        foreach (SessionInfo session in PhotonManager._PhotonManager.availableSessions)
        {
            GameObject sessionIntance = Instantiate(lobbyPrefab, viewportContent);
            sessionIntance.GetComponent<SessionEntry>().SetInfo(session);
        }
    }

    public void DestroyCanvasContent()
    {
        Debug.Log("Destroy Canvas");

        warningMessage.SetActive(PhotonManager._PhotonManager.availableSessions.Count <= 0);

        for (int i = 0; i < viewportContent.childCount; i++)
        {
            Destroy(viewportContent.GetChild(i).gameObject);
        }
    }

    public void UpdatePlayerCount(int number)
    {
        maxPlayerCount += number;

        maxPlayerCount = maxPlayerCount > 10? 1 : maxPlayerCount <= 0? 10 : maxPlayerCount; 

        maxPlayerCountText.text = maxPlayerCount.ToString();
    }
}
