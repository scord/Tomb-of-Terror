using UnityEngine;
using System.Collections;

public class LobbyManager : MonoBehaviour {

  [SerializeField] private MouseLook m_MouseLookScript;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    if (Input.GetButton("Fire2")) {
      m_MouseLookScript.enabled = true;
    } else {
      m_MouseLookScript.enabled = false;
    }
	}

  public void StartGame() {
    GameObject.Find("NetManager").GetComponent<NetManager>().StartFromLobby();
  }
}
