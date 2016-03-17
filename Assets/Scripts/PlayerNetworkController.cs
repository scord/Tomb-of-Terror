using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkController : NetworkBehaviour {


    [SerializeField] private Camera m_Camera;


	// Use this for initialization
	void Start () {
	    if (!isLocalPlayer)
        {

            GetComponent<PlayerController>().enabled = false;
            m_Camera.enabled = false;

        }
	}

}
