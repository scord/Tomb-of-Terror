using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkController : NetworkBehaviour {

  [SerializeField] Camera CharacterCam;

  // Use this for initialization
  void Start () {
    if (isLocalPlayer)
      {
        GetComponent<PlayerController>().enabled = true;
        Debug.Log("Here");
        CharacterCam.enabled = true;
      }
  }

}
