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
    //bool first = true;

    // Sets the message code to a value that is not already being used
    const short playerMsgType = MsgType.Highest + 1;
    
    class PlayerMsg : MessageBase
    {
        public short chosenPlayerIndex;
        public short controllerId;
    };
    

    // Called from the MenuController, starts game as a player/client
    public void JoinGame(int playerId, bool host, string ip)
    {
        chosenPlayerIndex = playerId; 
        NetworkManager.singleton.networkPort = 7777;
        NetworkManager.singleton.networkAddress = ip;
        Debug.Log(NetworkManager.singleton);
        if (host)
            NetworkManager.singleton.StartHost();
        else
            NetworkManager.singleton.StartClient();
        client.RegisterHandler(playerMsgType, OnPlayerRequest);
    }

    // Called when server is started
    public override void OnStartServer()
    {
        NetworkServer.RegisterHandler(playerMsgType, OnPlayerResponse);
        if (players.Length < 3) {
            Debug.LogError("Games need at least 3 Player Prefabs to start");
            return;
        }
        base.OnStartServer();
    }
    Maze maze;

    // Called when a client sends a message
    void OnPlayerResponse(NetworkMessage netMsg)
    {
        PlayerMsg msg = netMsg.ReadMessage<PlayerMsg>();
        Transform spawnTransform;
        switch(msg.chosenPlayerIndex) {
            case 0:
                chosenPlayer = players[0];
                spawnTransform = GameObject.Find("ServerSpawner").transform;

                break;
            case 1:
                chosenPlayer = players[1];
                spawnTransform = GameObject.Find("MummySpawner").transform;
                break;
            case 2:
                chosenPlayer = players[2];
                spawnTransform = GameObject.Find("ExplorerSpawner").transform;
                break;
            default:
                Debug.LogError("Uknown player index");
                return;

        }
        GameObject player = (GameObject)Instantiate(chosenPlayer, spawnTransform.position, spawnTransform.rotation);
        NetworkServer.AddPlayerForConnection(netMsg.conn, player, msg.controllerId);
        Debug.Log(chosenPlayer.name);
    }

    // Called when the server sends a message
    void OnPlayerRequest(NetworkMessage netMsg)
    {
        PlayerMsg msg = netMsg.ReadMessage<PlayerMsg>();
        msg.chosenPlayerIndex = (short)chosenPlayerIndex;
        client.Send(playerMsgType, msg);
    }

    // Called when a client attempts to join the server
    public override void  OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        PlayerMsg msg = new PlayerMsg();
        msg.controllerId = playerControllerId;
        NetworkServer.SendToClient(conn.connectionId, playerMsgType, msg);
    }


}
