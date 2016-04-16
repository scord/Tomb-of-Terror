using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ExplorerNetworkController : PlayerNetworkController {

  protected override void Start () {
    base.Start();
    if (!isLocalPlayer) GetComponent<Explorer_HeartRate>().enabled = false;
  }

}
