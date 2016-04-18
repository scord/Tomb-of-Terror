using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public int selected;
    //Text[] buttons;
    public NetManager manager;
    [SerializeField] private GameObject m_NetManagerPrefab;
    [SerializeField] private GameObject m_GameParamsPrefab;
    string ip;
    public bool host;
	// Use this for initialization
	void Start () {
        selected = 0;
        GameObject netManager = GameObject.Find("NetworkManager");
        if (netManager == null) {
            netManager = (GameObject) Instantiate(m_NetManagerPrefab, Vector3.zero, Quaternion.identity);
            netManager.GetComponent<NetworkManagerCustom>().SetupManager();
        }
        GameObject gameParams = GameObject.Find("GameParams") ?? (GameObject) Instantiate(m_GameParamsPrefab);
        gameParams.name = "GameParams";
        netManager.name = "NetworkManager";
        //manager = netManager.GetComponent<NetManager>();
	    //buttons = GetComponentsInChildren<Text>();
    }

    //bool pressed = false;
	// Update is called once per frame

    public void JoinGame(int playerID)
    {
        ip = transform.FindChild("IPAddressInput").GetComponent<InputField>().text;
        // Debug.Log(ip);

        GameObject.Find("GameParams").GetComponent<GameParams>().SetParams(playerID, false, ip);
        if (playerID == 1)
            manager.JoinGameMummy(playerID, false, ip);
        else
            manager.JoinGameExplorer(playerID, false, ip);
    }

    public void CreateGame(int playerID)
    {
        ip = transform.FindChild("IPAddressInput").GetComponent<InputField>().text;
        // Debug.Log(ip);
        GameObject.Find("GameParams").GetComponent<GameParams>().SetParams(playerID, true, ip);
        if (playerID == 1)
            manager.JoinGameMummy(playerID, true, ip);
        else
            manager.JoinGameExplorer(playerID, true, ip);
    }

    public void StartServer(int playerID) {
        ip = transform.FindChild("IPAddressInput").GetComponent<InputField>().text;

        manager.StartServerOnly(ip);
    }

    public void SetTutorial() {

    }
}
