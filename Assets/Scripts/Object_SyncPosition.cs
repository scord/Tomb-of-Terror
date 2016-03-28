using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Object_SyncPosition : NetworkBehaviour {

  [SyncVar (hook = "SyncPositionValues")] private Vector3 syncPos;
  [SyncVar (hook = "SyncChangingState")] private bool syncIsChanging = false;
  [SyncVar (hook = "SyncCarriedState")] private bool syncIsCarried = false;
  [SyncVar (hook = "SyncRotationValues")] private Quaternion syncRot;

  [SerializeField] private Transform m_Transform;
  [SerializeField] private Rigidbody m_Rigidbody;

  private bool isInteractedLocal = false;
  private string carriedBy;

  private float lerpRate;
  private float carringLerpRate = 18;
  private float throwingLerpRate = 28;

  private float carriedThreshold = 0.5f;
  private float changingThreshold = 0.01f;
  private float rotationThreshold = 5; //degrees

  private Vector3 lastPos;
  private Quaternion lastRot;

	// Use this for initialization
	void Start () {
    lerpRate = carringLerpRate;
    lastPos = m_Transform.position;
    lastRot = m_Transform.rotation;
  }

  void Update() {
    if ( syncIsChanging && !isInteractedLocal ) {
      if ( syncIsCarried ) {
        LerpPosition(carringLerpRate);
      } else {
        LerpPosition(throwingLerpRate);
        LerpRotation(throwingLerpRate);
      }
    }
  }
  void FixedUpdate () {
    if ( syncIsChanging && isInteractedLocal) {
      if ( syncIsCarried ) {
        TransmitPosition(carriedThreshold);
      } else {
        TransmitPosition(changingThreshold);
        TransmitRotation(rotationThreshold);
        if ( m_Transform.hasChanged) {
          m_Transform.hasChanged = false;
        }
        else {
          Rest();
        }
      }
    }
  }

  void LerpRotation(float rate) {
    Debug.Log(m_Transform.rotation);
    Debug.Log(syncRot);
    m_Transform.rotation = Quaternion.Lerp(m_Transform.rotation, syncRot, Time.deltaTime * rate);
  }
  void LerpPosition(float rate) {
    m_Transform.position = Vector3.Lerp(m_Transform.position, syncPos, Time.deltaTime * rate); 
  }

  [ClientCallback]
  public void PickUp(string uid) {
    carriedBy = uid;
    isInteractedLocal = true;
    CmdProvideChangingStateToServer(true);
    CmdProvideCarriedStateToServer(true);
  }

  [ClientCallback]
  public void Throw() {
    carriedBy = null;
    CmdProvideCarriedStateToServer(false);
  }

  [ClientCallback]
  private void Rest() {
    //isChanging = false;
    isInteractedLocal = false;
    CmdProvideChangingStateToServer(false);
    Debug.Log("Rested");
  }

  [Command]
  void CmdProvidePositionToServer(Vector3 pos) {
    syncPos = pos;
  }
  [Command]
  void CmdProvideChangingStateToServer(bool state) {
    syncIsChanging = state;
  }
  [Command]
  void CmdProvideCarriedStateToServer(bool state) {
    syncIsCarried = state;
  }
  [Command]
  void CmdProvideRotationsToServer(Quaternion rotation) {
    syncRot = rotation;
  }

  [ClientCallback]
  void TransmitPosition(float threshold) {
    if(Vector3.Distance(m_Transform.position, lastPos) > threshold ) {
      CmdProvidePositionToServer(m_Transform.position);
      lastPos = m_Transform.position;
    }
  }
  [ClientCallback]
  void TransmitRotation(float threshold) {
    if (Quaternion.Angle(m_Transform.rotation, lastRot) > threshold) {
      lastRot = m_Transform.rotation;
      CmdProvideRotationsToServer(lastRot);
    }
  }



  [Client]
  void SyncChangingState(bool newState) {
    syncIsChanging = newState;
    m_Rigidbody.isKinematic = syncIsChanging;
  }
  [Client]
  void SyncCarriedState(bool newState) {
    syncIsCarried = newState;
  }
  [Client]
  void SyncPositionValues(Vector3 latestPos) {
    syncPos = latestPos;
  }
  [Client]
  void SyncRotationValues(Quaternion latestRot) {
    syncRot = latestRot;
  }
	
}
