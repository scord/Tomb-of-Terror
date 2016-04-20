using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerController : NetworkBehaviour {

  // Use this for initialization
  [SerializeField] private MainCameraController m_Controller;
  [SerializeField] private MouseLook m_MouseLook;
  void Start () {
    if ( !isServer ) return;
    m_Controller.enabled = true;
    m_MouseLook = GameObject.Find("Main Camera").GetComponent<MouseLook>();
    m_MouseLook.enabled = true;
  }


}