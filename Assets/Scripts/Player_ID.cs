using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class Player_ID : NetworkBehaviour {

  [SyncVar] private string playerUniqueName;
  private NetworkInstanceId playerNetID;
  private Transform myTransform;
  public override void OnStartLocalPlayer() {
    GetNetIdentity();
    SetIdentity();
  }
  // Use this for initialization
  void Awake () {
    myTransform = transform;
  }
  
  // Update is called once per frame
  void Update () {
    if (myTransform.name == "" || myTransform.name.Contains("(Clone)")) {
      SetIdentity();
    } 
  }

  void SetIdentity() {
    if (!isLocalPlayer) {
      myTransform.name = playerUniqueName;
    } else {
      myTransform.name = MakeUniqueIdentity();
    }
  }

  [Client]
  void GetNetIdentity() {
    playerNetID = GetComponent<NetworkIdentity>().netId;
    CmdTellServerMyIdentity(MakeUniqueIdentity());
  }

  string MakeUniqueIdentity() {
    string uniqueIdentity = "Player-" + playerNetID.ToString();
    return uniqueIdentity;
  }

  [Command]
  void CmdTellServerMyIdentity(string name) {
    playerUniqueName = name;
  }
}
