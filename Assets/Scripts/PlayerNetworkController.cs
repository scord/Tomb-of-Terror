using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkController : NetworkBehaviour {

    [SerializeField] GameObject cam;

	// Use this for initialization
	void Start () {
	    if (isLocalPlayer)
        {
            GetComponent<OVRPlayerController>().enabled = true;
            cam.SetActive(true);
        }
	}

}
