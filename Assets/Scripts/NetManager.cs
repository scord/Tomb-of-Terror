using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetManager : NetworkManager
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    GameObject chosenPlayer;
    public Vector3 playerSpawnPos;
    int chosenPlayerIndex;
    bool first = true;

    // Sets the message code to a value that is not already being used
    const short playerMsgType = MsgType.Highest + 1;
    
    class PlayerMsg : MessageBase
    {
        public short chosenPlayerIndex;
        public short controllerId;
    };
    

    // Called from the MenuController, starts game as a player/client
    public void JoinGame(int playerId, bool host)
    {
        chosenPlayerIndex = playerId; 
        NetworkManager.singleton.networkPort = 7777;
        NetworkManager.singleton.networkAddress = "localhost";
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
        base.OnStartServer();
    }

    // Called when a client sends a message
    void OnPlayerResponse(NetworkMessage netMsg)
    {
        PlayerMsg msg = netMsg.ReadMessage<PlayerMsg>();
        Vector3 spawnPos;
        if (msg.chosenPlayerIndex == 1)
        {
            chosenPlayer = player1;
            spawnPos = GameObject.Find("MummySpawner").transform.position;
        } else
        {
            chosenPlayer = player2;
            spawnPos = GameObject.Find("RaiderSpawner").transform.position;
        }

        GameObject player = (GameObject)Instantiate(chosenPlayer, spawnPos, Quaternion.identity);
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
