using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkRunner runner; //es quien se encarga de enviar y recibir info, es mi medio de comunicacion con mi servidor
    [SerializeField] NetworkSceneManagerDefault sceneManager;
    [SerializeField] private Transform[] spawnPoint;
    public NetworkPrefabRef prefab; //Referencia del prefab

    Dictionary<PlayerRef, NetworkObject> players = new Dictionary<PlayerRef, NetworkObject>(); //PlayerRef es el ID  de nuestro jugador en la red, NetworkObject es el prefab de nuestro jugador

    [SerializeField] UnityEvent onPlayerJoined;

    #region Metodos de Photon

    /// <summary>
    /// 
    /// El siguiente paso es tener registrados que jugadores estan entrando
    /// 
    /// </summary>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Un nuevo jugador se unio.");

        if (runner.IsServer) //Solo la persona del host va a mandar a llamar este metodo
        {
            int randomSpawn = UnityEngine.Random.Range(0, spawnPoint.Length);
            NetworkObject networkPlayer = runner.Spawn(prefab, spawnPoint[randomSpawn].position, spawnPoint[randomSpawn].rotation, player);
            players.Add(player, networkPlayer); //Agregamos el diccionario el id del jugador y lo vinculamos con su prefab que acaba de instancearse
        }
        onPlayerJoined.Invoke(); //esto invoca el evento //Esta afuera para que todo jugador que entre se le apague el canvas
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;  
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (players.TryGetValue(player, out NetworkObject networkPlayer)) //si existe la referencia de ese jugador
        {
            runner.Despawn(networkPlayer); //elimino el objecto del jugador de la escena
            players.Remove(player); //lo elimino del diccionario
        }
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        //creo un objeto de tipo NetworkInputData
        NetworkInputData data = new NetworkInputData()
        {
            move = InputManager.Instance.GetMoveInput() == null ? new Vector2(0,0) : InputManager.Instance.GetMoveInput(),
            look = InputManager.Instance.GetMouseDelta(),
            isRunning = InputManager.Instance.WasRunInputPressed(),
            yRotation = Camera.main.transform.eulerAngles.y,
            shoot = InputManager.Instance.ShootInputPressed(),
        };

        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }
    #endregion

    /// <summary>
    /// 
    /// En este metodo vamoa a crear o buscar una partida. Si no existe alguna partida o lobby, entonces nosotros lo creamos y somos el host, 
    /// si ya hay una partida entonces entramos y comos el cliente.
    /// 
    /// Aqui vamos a configurar cuantos jugadores se pueden conectar a la partida como maximo, cual mapa va a ser el que va a cargar, 
    /// tambien si dentro de la partida puede haber cambios de escena
    /// 
    /// 
    /// </summary>
    private async void StartGame(GameMode mode)
    {
        runner.AddCallbacks(this);
        runner.ProvideInput = true;

        //guarda que escena se va a guardar
        var scene = SceneRef.FromIndex(0); //guardar una referencia a la escena 0

        //Guarda como se va a guardar las escenas en el juego, puede guardar la info de hasta 8 escenas
        var sceneInfo = new NetworkSceneInfo(); //creo una variable que me va a guardar las escenas que voy a usar y como las debo cargar

        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        await runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "#0001", //nombre interno que como yo desarrollador debo de entender
            Scene = scene,
            CustomLobbyName = "Official EA Europe", //nombre que quiero mostrar
            SceneManager = sceneManager
        });

    }

    public void StartGameAsHost()
    {
        StartGame(GameMode.Host);
    }

    public void StartGameAsClient()
    {
        StartGame(GameMode.Client);
    }
}
