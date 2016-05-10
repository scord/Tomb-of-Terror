using UnityEngine;
using System.Collections;

public class LobbyManager : MonoBehaviour {

  [SerializeField] private MouseLook m_MouseLookScript;
	// Use this for initialization

  void Awake() {
    m_MouseLookScript = GameObject.Find("Main Camera").GetComponent<MouseLook>();
  }


	// Update is called once per frame
	void Update () {
    if (!Cursor.visible) Cursor.visible = true;
    if (Input.GetButton("Fire2")) {
      m_MouseLookScript.enabled = true;
    } else {
      m_MouseLookScript.enabled = false;
    }
    if(Input.GetKey(KeyCode.Return)) {
      StartGame();
    }
	}

  public void StartGame() {
    GameObject.Find("NetworkManager").GetComponent<NetworkManagerCustom>().ServerStartMain();
  }


}
