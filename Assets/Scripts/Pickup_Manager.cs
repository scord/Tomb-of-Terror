using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Pickup_Manager : NetworkBehaviour {

  [SyncVar (hook = "SyncObjectIndex")] private int syncIndex = -1;
  [SyncVar (hook = "SyncChangingState")] private bool syncIsChanging = false;
  [SyncVar (hook = "SyncCarriedState")] private bool syncIsCarried = false;
  [SyncVar (hook = "SyncPositionValues")] private Vector3 syncPos;
  [SyncVar (hook = "SyncRotationValues")] private Quaternion syncRot;

  [SerializeField] private PlayerController m_PlayerController;
  private string prize_tag;
  private Transform m_Transform;
  private Rigidbody m_Rigidbody;
  private GameObject carriedObject;
  private Game_Manager_References GM_Ref;

  private float carringLerpRate = 18;
  //private float throwingLerpRate = 28;

  private float carriedThreshold = 0.5f;
  //private float changingThreshold = 0.01f;
  //private float rotationThreshold = 0.5f; //degrees

  private Vector3 lastPos;
  private Quaternion lastRot;

  private bool prizeTriggered = false;

  // Use this for initialization
  void Start () {
    m_PlayerController.EventPickUp += PickUpObject;
    m_PlayerController.EventThrow += ThrowObject;
    prize_tag = m_PlayerController.GetPrizeTag();
    GM_Ref = GameObject.Find("Game Manager").GetComponent<Game_Manager_References>();
  }

  void OnDisable() {
    m_PlayerController.EventPickUp -= PickUpObject;
    m_PlayerController.EventThrow -= ThrowObject;
  }

  void PickUpObject(GameObject go) {
    if (go.tag == prize_tag ) {
      CmdTriggerPrize();
      return;
    }
    PopulateVars(go);
    CmdProvideObjectIndex(GM_Ref.GetPickUpObjectIndex(carriedObject));
    CmdProvideChangingStateToServer(true);
    CmdProvideCarriedStateToServer(true);
  }

  void ThrowObject(GameObject go, Vector3 direction) {
    if ( go == carriedObject ) {
      m_Rigidbody.isKinematic = false;
      CmdProvideCarriedStateToServer(false);
      CmdThrowObject(direction);
      CmdProvideChangingStateToServer(false);
      carriedObject = null;
      m_Transform = null;
      m_Rigidbody = null;
    }
  }
  
  [Command]
  void CmdThrowObject(Vector3 direction) {
    m_Rigidbody.isKinematic = false;
    m_Rigidbody.AddForce(direction);
    m_Rigidbody.AddTorque(new Vector3(1, 1, 1));
    carriedObject = null;
    m_Transform = null;
    m_Rigidbody = null;
  }

  [Command]
  void CmdTriggerPrize() {
    Player_SyncRotation psr = GetComponent<Player_SyncRotation>();
    Transform cam = psr.camTransform;
    RaycastHit hit = new RaycastHit();
    if (!prizeTriggered && Physics.Raycast(cam.position, cam.TransformDirection(Vector3.forward), out hit, 50))
    {
      if ( hit.collider.gameObject.tag == prize_tag) {
        GameObject.Find("NetworkManager").GetComponent<NetworkManagerCustom>().EndGame();
        prizeTriggered = false;
      }
    }
  }

  void Update() {
    if ( carriedObject != null && !isLocalPlayer && syncIsChanging  ) {
      if ( syncIsCarried ) {
        LerpPosition(carringLerpRate);
      } else {
        //LerpPosition(throwingLerpRate);
        //LerpRotation(throwingLerpRate);
      }
    }
  }
  
  void PopulateVars(GameObject go) {
    carriedObject = go;
    m_Transform = go.GetComponent<Transform>();
    m_Rigidbody = go.GetComponent<Rigidbody>();
    m_Rigidbody.isKinematic = true;
    Debug.Log(go + "\n" + m_Transform + "\n" + m_Rigidbody);
  }

  void FixedUpdate () {
    if (carriedObject != null && isLocalPlayer && syncIsChanging) {
      if ( syncIsCarried ) {
        TransmitPosition(carriedThreshold);
      } else {
        //TransmitPosition(changingThreshold);
        //TransmitRotation(rotationThreshold);
        //if ( m_Transform.hasChanged ) {
        //  m_Transform.hasChanged = false;
        //} else {
        //  RestObject();
        //}
      }
    }
  }

  void LerpRotation(float rate) {
    Quaternion q1 = (m_Transform.rotation != Quaternion.identity) ? m_Transform.rotation : new Quaternion(0,0,0,1);
    Quaternion q2 = (syncRot != Quaternion.identity) ? syncRot : new Quaternion(0,0,0,1);
    m_Transform.rotation = Quaternion.Lerp(q1, q2, Time.deltaTime * rate);
  }
  void LerpPosition(float rate) {
    m_Transform.position = Vector3.Lerp(m_Transform.position, syncPos, Time.deltaTime * rate); 
  }

  [ClientCallback]
  private void RestObject() {
    CmdProvideChangingStateToServer(false);
    Debug.Log("Rested");
  }

  [Command]
  void CmdProvideObjectIndex(int newIndex) {
    syncIndex = newIndex;
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
  void SyncObjectIndex(int newIndex) {
    syncIndex = newIndex;
    if ( carriedObject == null) {
      PopulateVars(GM_Ref.GetPickUpObject(syncIndex));
    }
  }
  [Client]
  void SyncChangingState(bool newState) {
    syncIsChanging = newState;
    if (!newState) {
      if (m_Rigidbody != null) m_Rigidbody.isKinematic = newState;
      carriedObject = null;
      m_Rigidbody = null;
      m_Transform = null;
    }
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
