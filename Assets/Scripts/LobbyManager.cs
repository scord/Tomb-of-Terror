using UnityEngine;
using System.Collections;

public class LobbyManager : MonoBehaviour {

  [SerializeField] private MouseLook m_MouseLookScript;
  [SerializeField] private GameObject m_Canvas_1;
  [SerializeField] private GameObject m_Canvas_2;
	// Use this for initialization

  void Awake() {
    m_MouseLookScript = GameObject.Find("Main Camera").GetComponent<MouseLook>();
  }
	void Start () {
    m_Canvas_1.SetActive(true);	
    m_Canvas_2.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
    if (!Cursor.visible) Cursor.visible = true;
    if (Input.GetButton("Fire2")) {
      m_MouseLookScript.enabled = true;
    } else {
      m_MouseLookScript.enabled = false;
    }
	}

  public void StartGame() {
    GameObject.Find("NetworkManager").GetComponent<NetworkManagerCustom>().ServerStartMain();
  }

  public void OnDisable() {
    if (m_Canvas_1 != null) m_Canvas_1.SetActive(false);
    if (m_Canvas_2 != null) m_Canvas_2.SetActive(false);
  }

}
