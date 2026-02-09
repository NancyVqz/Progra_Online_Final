using Fusion;
using UnityEngine;


// Este script es el que va a actualizar mi lista de sesiones en el canvas
public class LobbyManager : MonoBehaviour
{
    [SerializeField] private Transform viewportContent;
    [SerializeField] private GameObject lobbyPrefab;

    private void Start()
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

    //private void OnDestroy() 
    //{
    //    if (PhotonManager._PhotonManager != null)
    //    {
    //        PhotonManager._PhotonManager.onSessionListUpdated -= DestroyCanvasContent;
    //        PhotonManager._PhotonManager.onSessionListUpdated -= UpdateSessionCanvas;
    //    }
    //}

    public void UpdateSessionCanvas()
    {
        DestroyCanvasContent();

        Debug.Log("Creando sesiones: " + PhotonManager._PhotonManager.availableSessions.Count);
        foreach(SessionInfo session in PhotonManager._PhotonManager.availableSessions)
        {
            GameObject sessionIntance = Instantiate(lobbyPrefab, viewportContent);
            sessionIntance.GetComponent<SessionEntry>().SetInfo(session);
        }

    }
    private void DestroyCanvasContent()
    {
        Debug.Log("Destroy Canvas");
        for (int i = 0; i < viewportContent.childCount; i++)
        {
            Destroy(viewportContent.GetChild(i).gameObject);
        }
    }
}
