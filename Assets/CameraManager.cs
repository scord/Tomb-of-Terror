using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CameraManager : MonoBehaviour {
	private int players = 0;
	private NetworkManagerCustom m_NetworkManager;
	[SerializeField] private MainCameraController m_CameraController;

	void Start(){
		Debug.Log("Starting");
		m_NetworkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManagerCustom>();
		m_NetworkManager.AddedPlayerCallback += AddRemovePlayer;
	}

	void OnDisable(){
		if( m_NetworkManager != null)
			m_NetworkManager.AddedPlayerCallback -= AddRemovePlayer;
	}

	void AddRemovePlayer(){
		Debug.Log("Added/Removed payer in camera manager");
		m_CameraController.SetCameras(FindObjectsOfType<PlayerController>());
	}
}
