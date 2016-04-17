using UnityEngine;
using System.Collections;

public class AutoChange : MonoBehaviour {

	// Use this for initialization

  [SerializeField] private GameObject m_NetManagerPrefab;
  [SerializeField] private GameObject m_NetManager;
  [SerializeField] private GameObject m_GameParamsObject;
  [SerializeField] private GameParams m_GameParams;
	void Start () {
    m_GameParamsObject = GameObject.Find("GameParams");
    m_GameParams = m_GameParamsObject.GetComponent<GameParams>();
    m_NetManager = GameObject.Find("NetManager");
    m_GameParams.LoadLobby();
    //m_NetManager.GetComponent<NetManager>().LoadLobby(int playerId, bool _shouldLoadMainLevel, bool _m_SkipTutorial, bool _loadMainAsHost, string _loadMainOnIp);
    //m_NetManager = (GameObject) Instantiate(m_NetManagerPrefab, Vector3.zero, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
