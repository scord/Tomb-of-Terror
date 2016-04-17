using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkController : NetworkBehaviour {


    [SerializeField] private Camera m_Camera;
    [SerializeField] private AudioListener m_Listener;
    [SyncVar] private bool isMainLevel;
    [SyncVar] public bool withPickupManager;

    // Use this for initialization
    virtual protected void Start () {
        if (isLocalPlayer) {
            GameObject.Find("Main Camera").SetActive(false);
            GetComponent<PlayerController>().StartConfig(isMainLevel);
            GetComponent<PlayerController>().pickupEnabled = withPickupManager;
        }
        if (!isLocalPlayer)
        {
            GetComponent<PlayerController>().enabled = false;
            GetComponent<OVRPlayerController>().enabled = false;
            m_Camera.enabled = false;
            m_Listener.enabled = false;
        }
	}

    public void SetMainLevel(bool newValue, bool newPickupValue) {
        isMainLevel = newValue;
        withPickupManager = newPickupValue;
        GetComponent<PlayerController>().StartConfig(isMainLevel);
        GetComponent<PlayerController>().pickupEnabled = withPickupManager;
    }

}
