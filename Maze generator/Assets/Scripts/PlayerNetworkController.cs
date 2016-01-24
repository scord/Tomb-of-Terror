using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkController : NetworkBehaviour {

    [SerializeField] Camera cam;

	// Use this for initialization
	void Start () {
	    if (isLocalPlayer)
        {
            GetComponent<PlayerController>().enabled = true;
            cam.enabled = true;
        }
	}

}
