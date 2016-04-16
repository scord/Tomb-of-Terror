using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetManager : NetworkManager
{
    [SerializeField] private GameObject[] players;
	public GameManager gameManager;

    GameObject chosenPlayer;
    public Vector3 playerSpawnPos;
    int chosenPlayerIndex;

    private bool shouldLoadMainLevel = false;
    //bool first = true;

    // Sets the message code to a value that is not already being used
    const short playerMsgType = MsgType.Highest + 1;
    const short changeLevelMsg = playerMsgType + 1;

    class PlayerMsg : MessageBase
    {
        public short chosenPlayerIndex;
        public short controllerId;
        public bool mainLevel = false;
    };


    // Called from the MenuController, starts game as a player/client
    public void JoinGameMummy(int playerId, bool host, string ip)
    {
        JoinGame(playerId, host, ip, 7776);
    }

    public void JoinGameExplorer(int playerId, bool host, string ip = "localhost") {
        Debug.Log(onlineScene);
        onlineScene = "startscene";
        Debug.Log(onlineScene);
        JoinGame(playerId, true, ip, 7775);
    }

    public void JoinGame(int playerId, bool host, string ip, int port = 7777) {
        chosenPlayerIndex = playerId;
        NetworkManager.singleton.networkPort = port;
        NetworkManager.singleton.networkAddress = ip;
        // Debug.Log(NetworkManager.singleton);
        if (host) {
            NetworkManager.singleton.StartHost();
        }
        else
            NetworkManager.singleton.StartClient();
        client.RegisterHandler(playerMsgType, OnPlayerRequest);
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
        Vector3 spawnPos;
        switch(msg.chosenPlayerIndex) {
            case 0:
                chosenPlayer = players[0];
                spawnPos = GameObject.Find("ServerSpawner").transform.position;
                break;
            case 1:
                chosenPlayer = players[1];
                spawnPos = GameObject.Find("MummySpawner").transform.position;
                break;
            case 2:
                chosenPlayer = players[2];
                spawnPos = GameObject.Find("ExplorerSpawner").transform.position;
                break;
            default:
                Debug.LogError("Uknown player index");
                return;

        }
        GameObject player = (GameObject)Instantiate(chosenPlayer, spawnPos, Quaternion.identity);
        //Debug.Log(player);
        if (player.GetComponent<PlayerController>() != null) {
           player.GetComponent<PlayerController>().StartConfig(msg.mainLevel); 
        }
        NetworkServer.AddPlayerForConnection(netMsg.conn, player, msg.controllerId);
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
        NetworkServer.SendToClient(conn.connectionId, playerMsgType, msg);
    }

    public void ChangeLevel(int _id) {
        PlayerMsg msg = new PlayerMsg();
        msg.controllerId = (short) _id;
        client.Send(changeLevelMsg, msg);
        //Network.CloseConnection(client, true);
    }

    public override void OnServerDisconnect(NetworkConnection conn) {
        Debug.Log(conn);
        StopHost();
    }

    void ClientChangeLevel(NetworkMessage netMsg) {
        //Debug.Log(netMsg);
        PlayerMsg msg = netMsg.ReadMessage<PlayerMsg>();
        offlineScene = "Loading";
        StopHost();
        shouldLoadMainLevel = true;
        onlineScene = "Sample";
        JoinGame(msg.controllerId, true, "");
        //JoinGame()
    }

}
