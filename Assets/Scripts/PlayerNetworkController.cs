using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkController : NetworkBehaviour {


    [SerializeField] private Camera m_Camera;
    [SerializeField]private AudioListener m_Listener;

    // Use this for initialization
    void Start () {
	    if (!isLocalPlayer)
        {

            GetComponent<PlayerController>().enabled = false;
            m_Camera.enabled = false;
            m_Listener.enabled = false;
        }
	}

}
