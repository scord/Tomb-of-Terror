using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSyncPosition : NetworkBehaviour {

  [SyncVar]
  private Vector3 syncPos;

  [SerializeField] Transform myTransform;
  [SerializeField] float lerpRate = 15;
  [SerializeField] float threshold = 0.5f;
  
  Vector3 lastPos;
  // float threshold = 0.05f;
  // Update is called once per frame

  void Start() {
    lastPos = myTransform.position;
  }
  void Update() {
    LerpPosition();
  }
  void FixedUpdate () {
    TransmitPosition();
  }

  void LerpPosition() {
    if(!isLocalPlayer) {
      myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate); 
    }
  }

  [Command]
  void CmdProvidePositionToServer(Vector3 pos) {
    syncPos = pos;
  }

  [ClientCallback]
  void TransmitPosition() {
    if(isLocalPlayer && Vector3.Distance(myTransform.position, lastPos) > threshold ) {
      CmdProvidePositionToServer(myTransform.position);
      lastPos = myTransform.position;
      Debug.Log("Does this work?");
    }
  }
}
