using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ExplorerNetworkController : NetworkBehaviour {

  [SerializeField] Camera CharacterCam;

  // Use this for initialization
  void Start () {
    if (isLocalPlayer)
      {
        GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        CharacterCam.enabled = true;
      }
  }

}
