using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkController : NetworkBehaviour {


    [SerializeField] private Camera m_Camera;
    [SerializeField] private AudioListener m_Listener;
    [SyncVar (hook="SyncIsMainLevel")] public bool isMainLevel;
    [SyncVar] public bool withPickupManager;

    private GameObject mainCamera;

    // Use this for initialization
    virtual protected void Start () {
        if (isLocalPlayer) {
            mainCamera = GameObject.Find("Main Camera");
            MouseLook ml = (mainCamera != null) ? mainCamera.GetComponent<MouseLook>() : null;
            if (ml != null) ml.enabled = false;
            if (mainCamera != null) mainCamera.SetActive(false);
            ml = null;
            //pc.pickupEnabled = withPickupManager;
        }
        if (!isLocalPlayer)
        {
			ExplorerMusicController emc = GetComponent<ExplorerMusicController>();
			if (emc != null){
				emc.enabled = false;
			}
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

    [Client]
    private void SyncIsMainLevel(bool newValue) {
        isMainLevel = newValue;
        gameObject.GetComponent<PlayerController>().StartConfig(isMainLevel);
    }

    private void OnDisable() {
        if (isLocalPlayer) {
            if (mainCamera != null) mainCamera.SetActive(true);
        }
    }

}
