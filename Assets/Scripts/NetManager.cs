using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetManager : NetworkManager
{
    [SerializeField] private GameObject[] players;
    [SerializeField] private string m_MummyIntroScene;
    [SerializeField] private string m_ExplorerIntroScene;
    [SerializeField] private string m_MainScene;
    [SerializeField] private string m_LoadingScene;
    [SerializeField] private string m_LobbyScene;
    [SerializeField] private string m_EndScene;
    [SerializeField] private string m_MenuScene;
	public GameManager gameManager;

    GameObject chosenPlayer;
    public Vector3 playerSpawnPos;
    int chosenPlayerIndex;

    [SerializeField] private bool shouldLoadMainLevel = false;
    [SerializeField] private bool m_SkipTutorial = false;
    private bool loadMainAsHost = false;
    private string loadMainOnIp = "";
    //bool first = true;

    private const int default_port = 7777;
    // Sets the message code to a value that is not already being used
    const short playerMsgType = MsgType.Highest + 1;
    const short changeLevelMsg = playerMsgType + 1;

    class PlayerMsg : MessageBase
    {
        public short chosenPlayerIndex;
        public short controllerId;
        public bool mainLevel = false;
        public bool withPickup = true;
    };

    // Called from the MenuController, starts game as a player/client
    public void JoinGameMummy(int playerId, bool host, string ip)
    {
        loadMainAsHost = host;
        loadMainOnIp = ip;
        onlineScene = m_MummyIntroScene;
        if( m_SkipTutorial ) {
            JoinMainGame(playerId);
        } else {
            JoinGame(playerId, true, "", 7776);
        }
    }

    public void JoinMainGame(int playerId) {
        if ( m_SkipTutorial ) {
            onlineScene = m_MainScene;
            shouldLoadMainLevel = true;
        } else if (loadMainAsHost) {
            onlineScene = m_MainScene;
        }
        if ( shouldLoadMainLevel ) {
            JoinGame(playerId, loadMainAsHost, loadMainOnIp);
        }
    }

    public void StartServerOnly(string ip) {
        onlineScene = m_LobbyScene;
        NetworkManager.singleton.networkPort = default_port;
        NetworkManager.singleton.networkAddress = ip;
        if (NetworkManager.singleton.StartServer()) {
            Debug.Log("Server started");
        }
    }

    public void JoinGameExplorer(int playerId, bool host, string ip = "localhost") {
        loadMainAsHost = host;
        loadMainOnIp = ip;
        onlineScene = m_ExplorerIntroScene;
        if ( m_SkipTutorial ) {
            JoinMainGame(playerId);
        } else {
            JoinGame(playerId, true, "", 7775);
        }
    }

    public void JoinGame(int playerId, bool host, string ip, int port = default_port) {
        chosenPlayerIndex = playerId;
        NetworkManager.singleton.networkPort = port;
        NetworkManager.singleton.networkAddress = ip;
        if (host) {
            NetworkManager.singleton.StartHost();
        }
        else {
            NetworkManager.singleton.StartClient();
        }
        client.RegisterHandler(playerMsgType, OnPlayerRequest);
    }

    public void StartFromLobby() {
        if (onlineScene == m_LobbyScene) {
            Debug.Log(numPlayers);
            NetworkManager.singleton.ServerChangeScene(m_MainScene);
        }
    }

    // Called when server is started
    public override void OnStartServer()
    {
        NetworkServer.RegisterHandler(playerMsgType, OnPlayerResponse);
        NetworkServer.RegisterHandler(changeLevelMsg, ClientChangeLevel);
        if (players.Length < 3) {
            Debug.LogError("Games need at least 3 Player Prefabs to start");
            return;
        }
        base.OnStartServer();
    }

    // Called when a client sends a message
    void OnPlayerResponse(NetworkMessage netMsg)
    {
        PlayerMsg msg = netMsg.ReadMessage<PlayerMsg>();
        Transform spawnTransform;
        switch(msg.chosenPlayerIndex) {
            case 0:
                playerPrefab = players[0];
                spawnTransform = GameObject.Find("ServerSpawner").transform;

                break;
            case 1:
                playerPrefab = players[1];
                spawnTransform = GameObject.Find("MummySpawner").transform;
                break;
            case 2:
                playerPrefab = players[2];
                spawnTransform = GameObject.Find("ExplorerSpawner").transform;
                break;
            default:
                Debug.LogError("Uknown player index");
                return;

        }
        RegisterStartPosition(spawnTransform);

        base.OnServerAddPlayer(netMsg.conn, msg.controllerId);

        UnRegisterStartPosition(spawnTransform);

        //GameObject player = (GameObject)Instantiate(chosenPlayer, spawnTransform.position, spawnTransform.rotation);
        //Debug.Log(player);
       // if (player.GetComponent<PlayerNetworkController>() != null) {
       //    player.GetComponent<PlayerNetworkController>().SetMainLevel(msg.mainLevel, msg.withPickup); 
       // }
        //base.OnServerAddPlayer()
        //NetworkServer.AddPlayerForConnection(netMsg.conn, player, msg.controllerId);
        // Debug.Log(chosenPlayer.name);
    }

    // Called when the server sends a message
    void OnPlayerRequest(NetworkMessage netMsg)
    {
        PlayerMsg msg = netMsg.ReadMessage<PlayerMsg>();
        msg.chosenPlayerIndex = (short)chosenPlayerIndex;
        msg.mainLevel = shouldLoadMainLevel;
        client.Send(playerMsgType, msg);
    }

    // Called when a client attempts to join the server
    public override void  OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        PlayerMsg msg = new PlayerMsg();
        msg.controllerId = playerControllerId;
        msg.withPickup = !( onlineScene == m_LobbyScene || onlineScene == m_EndScene);
        NetworkServer.SendToClient(conn.connectionId, playerMsgType, msg);
    }

    public void ChangeLevel(int _id) {
        PlayerMsg msg = new PlayerMsg();
        msg.controllerId = (short) _id;
        client.Send(changeLevelMsg, msg);
        //Network.CloseConnection(client, true);
    }

    void ClientChangeLevel(NetworkMessage netMsg) {
        PlayerMsg msg = netMsg.ReadMessage<PlayerMsg>();
        offlineScene = m_LoadingScene;
        //StopHost();
        Shutdown();
        Destroy(gameObject);
        //shouldLoadMainLevel = true;
        //onlineScene = m_LobbyScene;
        //JoinMainGame(msg.controllerId);
    }

    public void LoadLobby(int playerId, bool _shouldLoadMainLevel, bool _m_SkipTutorial, bool _loadMainAsHost, string _loadMainOnIp) {
        shouldLoadMainLevel = _shouldLoadMainLevel;
        m_SkipTutorial = _m_SkipTutorial;
        loadMainAsHost = _loadMainAsHost;
        loadMainOnIp = _loadMainOnIp;
        if ( _loadMainAsHost ) {
            onlineScene = m_MainScene;
            JoinGame(playerId, loadMainAsHost, loadMainOnIp);
        } else {
            JoinMainGame(playerId);
        }
    }

}
