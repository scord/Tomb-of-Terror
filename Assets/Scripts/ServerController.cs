using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerController : NetworkBehaviour {

  // Use this for initialization
  [SerializeField] private MainCameraController m_Controller;
  [SerializeField] private MouseLook m_MouseLook;
  [SerializeField] private GameObject m_Canvas;
  void Start () {
    if ( isClient ) return;
    //if ( isLocalPlayer ) return;
    m_Canvas.SetActive(true);
    m_Controller.enabled = true;
    m_MouseLook = GameObject.Find("Main Camera").GetComponent<MouseLook>();
    m_MouseLook.enabled = true;
  }


}