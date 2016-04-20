using UnityEngine;
using System.Collections;

public class GameParams : MonoBehaviour {

    [SerializeField] private bool shouldLoadMainLevel = false;
    [SerializeField] private bool m_SkipTutorial = false;
    [SerializeField] private bool loadMainAsHost = false;
    [SerializeField] private string loadMainOnIp = "";
    [SerializeField] private int playerId;
    [SerializeField] private bool canPickup;
    //bool first = true;

    public bool mainLevel { get { return shouldLoadMainLevel; } set { shouldLoadMainLevel = value;}}
    private const int default_port = 7777;
	// Use this for initialization
	void Start () {
    DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void SetParams(int pId, bool host, string ip) {
    playerId = pId;
    loadMainAsHost = host;
    loadMainOnIp = ip;
  }

  public void LoadLobby() {
    shouldLoadMainLevel = true;
    GameObject.Find("NetManager").GetComponent<NetManager>().LoadLobby(playerId, shouldLoadMainLevel, false, loadMainAsHost, loadMainOnIp);
  }

  public bool GetMainLevel() {
    return shouldLoadMainLevel;
  }

  public bool GetCanPickup() {
    return canPickup;
  }

  public void SetCanPickup(bool newValue) {
    canPickup = newValue;
  }

}
