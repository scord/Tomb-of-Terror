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

    const short playerMsgType = MsgType.Highest + 1;
    
    class PlayerMsg : MessageBase
    {
        public short chosenPlayerIndex;
        public short controllerId;
    };
    
    
    public void CreateGame()
    {
        NetworkManager.singleton.networkPort = 7777;
        NetworkManager.singleton.networkAddress = "localhost";
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame(int playerId)
    {
        
        chosenPlayerIndex = playerId; 
        NetworkManager.singleton.networkPort = 7777;
        NetworkManager.singleton.networkAddress = "localhost";
        NetworkManager.singleton.StartClient();
        client.RegisterHandler(playerMsgType, OnPlayerRequest);
    }

    public override void OnStartServer()
    {
        NetworkServer.RegisterHandler(playerMsgType, OnPlayerResponse);
        base.OnStartServer();
    }

    void OnPlayerResponse(NetworkMessage netMsg)
    {
        PlayerMsg msg = netMsg.ReadMessage<PlayerMsg>();
        if (msg.chosenPlayerIndex == 1)
        {
            chosenPlayer = player1;
        } else
        {
            chosenPlayer = player2;
        }

        GameObject player = (GameObject)Instantiate(chosenPlayer, playerSpawnPos, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(netMsg.conn, player, msg.controllerId);
        Debug.Log(chosenPlayer.name);
    }

    void OnPlayerRequest(NetworkMessage netMsg)
    {
        PlayerMsg msg = netMsg.ReadMessage<PlayerMsg>();
        msg.chosenPlayerIndex = (short)chosenPlayerIndex;
        client.Send(playerMsgType, msg);
    }


    public override void  OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        if (first)
        {
            GameObject player = (GameObject)Instantiate(player3, playerSpawnPos, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
            first = false;
        }
        else
        {
            PlayerMsg msg = new PlayerMsg();
            msg.controllerId = playerControllerId;
            NetworkServer.SendToClient(conn.connectionId, playerMsgType, msg);
        }
    }

}
