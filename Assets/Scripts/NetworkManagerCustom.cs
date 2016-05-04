using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

using PC = UnityEngine.Networking.PlayerController;

public class NetworkManagerCustom : NetworkManager {
  [SerializeField] private bool shouldLoadMainLevel = false;
  private bool m_SkipTutorial = false;

  [SerializeField] private string m_MummyIntroScene;
  [SerializeField] private string m_ExplorerIntroScene;
  [SerializeField] private string m_MainScene;
  [SerializeField] private string m_LoadingScene;
  [SerializeField] private string m_LobbyScene;
  [SerializeField] private string m_EndScene;
  [SerializeField] private string m_MenuScene;

  [SerializeField] private GameObject[] m_PlayerPrefabs;

  [SerializeField] private string[] m_PlayerSpawnName;

  public delegate void PlayerAdded();
  public event PlayerAdded AddedPlayerCallback;

  private int m_choosenIndex;
  public int choosenIndex { get {return m_choosenIndex;} set { m_choosenIndex = value;}}
  private bool loadMainAsHost = false;
  private string loadMainOnIp = null;
  private bool alreadyStartedMain = false;

  private const int default_port = 7777;

  const short choosenPlayerMsg = MsgType.Highest + 1;

  class PlayerMsg : MessageBase {
    public short choosenPlayerIndex;
    public short controllerId;
  }

  public override void OnServerRemovePlayer(NetworkConnection conn, UnityEngine.Networking.PlayerController player){
    base.OnServerRemovePlayer(conn, player);
    Debug.Log("SERVER REMOVE PLATTE");
    if( null != AddedPlayerCallback )
      AddedPlayerCallback();
  }


  void OnPlayerResponse(NetworkMessage netMsg) {
    PlayerMsg msg = netMsg.ReadMessage<PlayerMsg>();
    Transform spawnTransform = null;
    playerPrefab = m_PlayerPrefabs[msg.choosenPlayerIndex];
    GameObject go = GameObject.Find(m_PlayerSpawnName[msg.choosenPlayerIndex]);
    if ( go != null ) spawnTransform = go.transform;
    RegisterStartPosition(spawnTransform);
    base.OnServerAddPlayer(netMsg.conn, msg.controllerId);
    UnRegisterStartPosition(spawnTransform);

    if( null != AddedPlayerCallback )
      AddedPlayerCallback();

    if ( netMsg.conn != null  && netMsg.conn.clientOwnedObjects != null){
      foreach(NetworkInstanceId netId in netMsg.conn.clientOwnedObjects) {
        GameObject g = NetworkServer.FindLocalObject(netId);
        PlayerNetworkController pnc = null;
        if (go != null) g.GetComponent<PlayerNetworkController>();
        if ( pnc != null ) pnc.SetMainLevel(shouldLoadMainLevel, !( onlineScene == m_LobbyScene || onlineScene == m_EndScene));
      }
    }
  }
  public override void  OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
    PlayerMsg msg = new PlayerMsg();
    msg.controllerId = playerControllerId;
    NetworkServer.SendToClient(conn.connectionId, choosenPlayerMsg, msg);
  }



  private void NewChangleLevel(int _id) {
    offlineScene = m_LoadingScene;
    StopGameConnection();
    if (loadMainAsHost) {
      onlineScene = m_MainScene;
    } else {
      onlineScene = m_LobbyScene;
    }
    JoinMain();
    offlineScene = m_MenuScene;
  }

  private void OldChangeLevel(int _id) {
    offlineScene = m_LoadingScene;
    NetworkManager.singleton.StopClient();
    onlineScene = m_LobbyScene;
    NetworkManager.singleton.StartClient();
    offlineScene = m_MenuScene;
  }

  public void ChangeLevel() {
    offlineScene = m_LoadingScene;
    StopGameConnection();
    shouldLoadMainLevel = true;
    GameObject.Find("GameParams").GetComponent<GameParams>().mainLevel = shouldLoadMainLevel;
    StartCoroutine(ConnectToLobby());
    offlineScene = m_MenuScene;

  }

  private IEnumerator ConnectToLobby() {
    yield return new WaitForSeconds(0.3f);
    if ( loadMainAsHost ) {
      HostGame();
    } else {
      JoinGame();
    }
  }

  public override void OnStartServer() {
    //onlineScene = m_ExplorerIntroScene;
    NetworkServer.RegisterHandler(choosenPlayerMsg, OnPlayerResponse);
    base.OnStartServer();
  }

  private void JoinMain() {
    SetPort();
    SetIPAdress(loadMainOnIp);
    if ( loadMainAsHost ) {
      NetworkManager.singleton.StartHost();
    } else {
      NetworkManager.singleton.StartClient();
    }
  }

  private void JoinLobby() {
    onlineScene = m_LobbyScene;
    SetPort();
    SetIPAdress(loadMainOnIp);
    NetworkManager.singleton.StartClient();
  }

  private bool gameEnded = false;
  public void EndGame(){
    if (!gameEnded ) {
      SceneManager.LoadScene(m_EndScene);
      NetworkManager.singleton.ServerChangeScene(m_EndScene);
      onlineScene = m_EndScene;
      gameEnded = true;
    }
  }

  public void ManageFinalState() {
    if (onlineScene == m_MainScene) {
      EndGame();
    } else if ( (onlineScene == m_ExplorerIntroScene) || (onlineScene == m_MummyIntroScene) ){
      ChangeLevel();
    }
  }

  public void ServerStartMain() {
    if (!alreadyStartedMain && (onlineScene == m_LobbyScene)) {
      SceneManager.LoadScene(m_MainScene);
      NetworkManager.singleton.ServerChangeScene(m_MainScene);
      onlineScene = m_MainScene;
      alreadyStartedMain = true;
    }
  }

  private void OldJoinGame() {
    if (client == null || client.connection == null ||client.connection.connectionId == -1) {
      SetIPAdress(loadMainOnIp ?? GetIPAdress());
      SetPort();
      NetworkManager.singleton.StartClient();
      client.RegisterHandler(choosenPlayerMsg, OnPlayerRequest);
    } else {
      NetworkManager.singleton.StopClient();
    }
  }

  private void NewJoinGame() {
    loadMainAsHost = false;
    loadMainOnIp = GetIPAdress();
    NetworkManager.singleton.StartHost();
    client.RegisterHandler(choosenPlayerMsg, OnPlayerRequest);
  }
  public void JoinGame() {
    if (shouldLoadMainLevel) {
        SceneManager.LoadScene(m_LoadingScene);
        onlineScene = m_LobbyScene;
        OldJoinGame();
      } else {
        NewJoinGame();
      }
  }

  public override void OnServerSceneChanged(string newSceneName) {
    onlineScene = newSceneName;
  }

  private void NewHostGame() {
    loadMainAsHost = true;
    loadMainOnIp = GetIPAdress();
    //SetPort(7776);
    //onlineScene = m_ExplorerIntroScene;
    NetworkManager.singleton.StartHost();
  }

  public void JoinGameMummy() {
    choosenIndex = 1;
    onlineScene = m_MummyIntroScene;
    SetPort(7776);
    if (m_SkipTutorial) {
      shouldLoadMainLevel = true;
      GameObject.Find("GameParams").GetComponent<GameParams>().mainLevel = shouldLoadMainLevel;
    }
    JoinGame();
  }

  public void JoinGameExplorer() {
    choosenIndex = 0;
    onlineScene = m_ExplorerIntroScene;
    SetPort(7775);
    if (m_SkipTutorial) {
      shouldLoadMainLevel = true;
      GameObject.Find("GameParams").GetComponent<GameParams>().mainLevel = shouldLoadMainLevel;
    }
    JoinGame();
  }

  public void HostGameMummy() {
    choosenIndex = 1;
    onlineScene = m_MummyIntroScene;
    SetPort(7776);
    if (m_SkipTutorial) {
      shouldLoadMainLevel = true;
      GameObject.Find("GameParams").GetComponent<GameParams>().mainLevel = shouldLoadMainLevel;
    }
    HostGame();
  }

  public void HostGameExplorer() {
    choosenIndex = 0;
    onlineScene = m_ExplorerIntroScene;
    SetPort(7775);
    if (m_SkipTutorial) {
      shouldLoadMainLevel = true;
      GameObject.Find("GameParams").GetComponent<GameParams>().mainLevel = shouldLoadMainLevel;
    }
    HostGame();
  }

  private void OldHostGame() {
    SetPort();
    NetworkManager.singleton.StartHost();
  }

  public void HostGame() {
    if (shouldLoadMainLevel) {
        onlineScene = m_MainScene;
        OldHostGame();
      } else {
        NewHostGame();
      }
    client.RegisterHandler(choosenPlayerMsg, OnPlayerRequest);
  }

  void OnPlayerRequest(NetworkMessage netMsg) {
    PlayerMsg msg = netMsg.ReadMessage<PlayerMsg>();
    msg.choosenPlayerIndex = (short) choosenIndex;
    client.Send(choosenPlayerMsg, msg);

  }

  private void NewServerOnly() {
    SetPort();
    onlineScene = m_LobbyScene;
    NetworkManager.singleton.StartServer();
  }

  private void OldServerOnly() {
    SetPort();
    NetworkManager.singleton.StartServer();
  }

  public void ServerOnly() {
    NewServerOnly();
    //OldServerOnly();
  }

  public override void OnClientConnect(NetworkConnection conn) {
    base.OnClientConnect(conn);
  }

  public override void OnClientSceneChanged(NetworkConnection conn) {
    //StartCoroutine(CLientSceneWithWait(conn));
    base.OnClientSceneChanged(conn);
  }

  private IEnumerator CLientSceneWithWait(NetworkConnection conn) {
    yield return new WaitForSeconds(0.3f);
  }
  private void SetIPAdress(string ipadd) {
    NetworkManager.singleton.networkAddress = ipadd;
  }

  private string GetIPAdress() {
    return GameObject.Find("InputFieldIPAddress").transform.FindChild("Text").GetComponent<Text>().text;
  }

  private void SetPort(int port = 7777) {
    NetworkManager.singleton.networkPort = port;
  }

  private void OnLevelWasLoaded(int level) {
    if (level == 0) {
      SetupMenuSceneBUttons();
      ResetContext();
    } else if ( level == 3 ){
      StartCoroutine(SetupDisconnectButton());
    }
  }

  private void SetupMenuSceneBUttons() {

    GameObject.Find("ButtonStartHostMummy").GetComponent<Button>().onClick.RemoveAllListeners();
    GameObject.Find("ButtonStartHostMummy").GetComponent<Button>().onClick.AddListener(HostGameMummy);

    GameObject.Find("ButtonJoinGameMummy").GetComponent<Button>().onClick.RemoveAllListeners();
    GameObject.Find("ButtonJoinGameMummy").GetComponent<Button>().onClick.AddListener(JoinGameMummy);

    GameObject.Find("ButtonStartHostExplorer").GetComponent<Button>().onClick.RemoveAllListeners();
    GameObject.Find("ButtonStartHostExplorer").GetComponent<Button>().onClick.AddListener(HostGameExplorer);

    GameObject.Find("ButtonJoinGameExplorer").GetComponent<Button>().onClick.RemoveAllListeners();
    GameObject.Find("ButtonJoinGameExplorer").GetComponent<Button>().onClick.AddListener(JoinGameExplorer);

    GameObject.Find("ButtonStartServer").GetComponent<Button>().onClick.RemoveAllListeners();
    GameObject.Find("ButtonStartServer").GetComponent<Button>().onClick.AddListener(ServerOnly);

    GameObject.Find("ToggleSkipTutorial").GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
    GameObject.Find("ToggleSkipTutorial").GetComponent<Toggle>().onValueChanged.AddListener(ToggleSkipTutorial);
  }

  void ToggleSkipTutorial(bool newValue) {
    m_SkipTutorial = newValue;
  }

  private void ResetContext() {
    shouldLoadMainLevel = false;
    GameObject.Find("GameParams").GetComponent<GameParams>().mainLevel = shouldLoadMainLevel;
    m_SkipTutorial = false;
    alreadyStartedMain = false;
    loadMainAsHost = false;
    gameEnded = false;
    loadMainOnIp = null;
    GameObject go = GameObject.Find("HeartRate");
    if ( go != null ) Destroy(go);
  }

  public void CloseGameConnection() {
    offlineScene = m_MenuScene;
    StopGameConnection();
    ResetContext();
    StartCoroutine(CheckFirstLevel());
    //onlineScene = m_ExplorerIntroScene;
  }

  private IEnumerator CheckFirstLevel() {
    yield return new WaitForSeconds(0.5f);
    if (SceneManager.GetActiveScene().name != m_MenuScene) {
      SceneManager.LoadScene(m_MenuScene);
    }
  }

  private void StopGameConnection() {
    NetworkManager.singleton.StopHost();
    NetworkServer.Reset();
  }

  private void SetupLoadingSceneButtons() {
    GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.RemoveAllListeners();
    GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.AddListener(HostGame);

    GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.RemoveAllListeners();
    GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.AddListener(JoinGame);

    SetupDisconnectButton();

  }

  private IEnumerator SetupDisconnectButton() {
    yield return new WaitForSeconds(0.3f);
    if (SceneManager.GetActiveScene().name == m_LoadingScene) {
      GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.RemoveAllListeners();
      GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.AddListener(CloseGameConnection);
    }
  }

  public void SetupManager() {
    SetupMenuSceneBUttons();
  }

  public void EscapeKeyPressed() {
    if ( SceneManager.GetActiveScene().name == m_MenuScene) {
      Application.Quit();
    } else {
      CloseGameConnection();
    }
  }

}
