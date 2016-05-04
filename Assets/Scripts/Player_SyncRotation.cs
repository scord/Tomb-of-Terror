using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_SyncRotation : NetworkBehaviour {

  [SyncVar (hook = "OnPlayerRotSynced")] private float syncPlayerRotation;
  [SyncVar (hook = "OnCamRotSynced")] private float syncCamRotation;

  [SerializeField] private Transform m_PlayerTransform;
  [SerializeField] private Transform m_CamTransform;

  public Transform camTransform { get { return m_CamTransform; } }

  private float lerpRate = 20;

  private float lastPlayerRotation;
  private float lastCameraRotation;
  private float threshold = 1;
  
  //private List<float> syncPlayerRotationList = new List<float>();
  //private List<float> syncCamRotationList = new List<float>();

  //private float closeEnough = 0.4f;
  //[SerializeField] private bool useHistoricInterpolation;
	// Use this for initialization
	void Start () {
    if(isLocalPlayer) {
      lastPlayerRotation = m_PlayerTransform.localEulerAngles.y;
      lastCameraRotation = m_CamTransform.localEulerAngles.x;
      LerpRotations();
    }
	}
	
	// Update is called once per frame
	void Update () {
    LerpRotations();
	}

  void FixedUpdate() {
    TransmitRotations();
  }

  void LerpRotations() {
    if(!isLocalPlayer) {
      LerpPlayerRotation(syncPlayerRotation);
      LerpCamRotation(syncCamRotation);
    }
  }

  void LerpCamRotation(float rot) {
    Vector3 camNewRot = new Vector3(rot, 0, 0);
    m_CamTransform.localRotation = Quaternion.Lerp(m_CamTransform.localRotation, Quaternion.Euler(camNewRot), lerpRate*Time.deltaTime);
  }

  void LerpPlayerRotation(float rot) {
    Vector3 playerNewRot = new Vector3(0, rot, 0);
    m_PlayerTransform.rotation = Quaternion.Lerp(m_PlayerTransform.rotation, Quaternion.Euler(playerNewRot), lerpRate*Time.deltaTime);
  }

  [Command]
  void CmdProvideRotationsToServer(float playerRot, float camRot) {
    syncPlayerRotation = playerRot;
    syncCamRotation = camRot;
  }

  [ClientCallback]
  void TransmitRotations() {
    if (isLocalPlayer && (CheckIfBeyondThreshold(m_PlayerTransform.localEulerAngles.y, lastPlayerRotation) || CheckIfBeyondThreshold(m_CamTransform.localEulerAngles.x, lastCameraRotation))) {
      lastPlayerRotation = m_PlayerTransform.localEulerAngles.y;
      lastCameraRotation = m_CamTransform.localEulerAngles.x;
      CmdProvideRotationsToServer(lastPlayerRotation, lastCameraRotation);
    }
  }

  bool CheckIfBeyondThreshold(float rotation1, float rotation2) {
    return (Mathf.Abs(rotation1-rotation2) > threshold);
  }

  [Client]
  void OnPlayerRotSynced(float latestPlayerRot) {
    syncPlayerRotation = latestPlayerRot;
    //syncPlayerRotationList.Add(syncPlayerRotation);
  }

  [Client]
  void OnCamRotSynced(float latestCamRot) {
    syncCamRotation = latestCamRot;
    //syncCamRotationList.Add(syncCamRotation);
  }
}
