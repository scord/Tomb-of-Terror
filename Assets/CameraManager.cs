using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CameraManager : MonoBehaviour {
	private int players = 0;
	private NetworkManagerCustom m_NetworkManager;
	[SerializeField] private MainCameraController m_CameraController;

	void Start(){
		m_NetworkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManagerCustom>();
		m_NetworkManager.AddcameraPosition += AddRemovePlayer;
	}

	void OnDisable(){
		if( m_NetworkManager != null)
			m_NetworkManager.AddcameraPosition -= AddRemovePlayer;
	}

	void AddRemovePlayer(){
		// Debug.Log("Added/Removed payer in camera manager");
		m_CameraController.SetCameras(FindObjectsOfType<PlayerController>());
	}
}
