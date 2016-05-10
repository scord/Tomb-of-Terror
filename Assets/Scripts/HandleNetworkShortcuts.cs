using UnityEngine;
using System.Collections;

public class HandleNetworkShortcuts : MonoBehaviour {


  [SerializeField] private NetworkManagerCustom m_NetworkManager;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace)) {
      m_NetworkManager.EscapeKeyPressed();
    }
	}
}
