using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[NetworkSettings(channel = 0, sendInterval = 0.2f)]
public class Player_SyncPosition : NetworkBehaviour {

  [SyncVar (hook = "SyncPositionValues")] private Vector3 syncPos;
  [SerializeField] Transform m_Transform;
  private Animator m_Animator;

  private float lerpRate = 10;

  public AudioSource footStep1;
  public AudioSource footStep2;

  bool leftFoot = true;

  private Vector3 lastPos;
  private float threshold = 0.3f;
	// Use this for initialization
	void Start () {
    lastPos = m_Transform.position;
    if (!isLocalPlayer) {
      m_Animator = GetComponent<Animator>();
    } else {
      CmdSendPositionToServer(m_Transform.position);
      lastPos = m_Transform.position;
    }
	}

  void FixedUpdate() {
    TransmitPositions();
  }

	// Update is called once per frame
	void Update () {
    LerpPosition();
	}

  void LerpPosition() {
    if(!isLocalPlayer) {
      m_Transform.position = Vector3.Lerp(m_Transform.position, syncPos, Time.deltaTime * lerpRate);
		}
  }

  [Command]
  void CmdSendPositionToServer(Vector3 pos) {
    syncPos = pos;
  }

  [ClientCallback]
  void TransmitPositions() {
    if(isLocalPlayer && Vector3.Distance(m_Transform.position, lastPos) > threshold ) {
      CmdSendPositionToServer(m_Transform.position);
      lastPos = m_Transform.position;
    }
  }

  [Client]
  void SyncPositionValues(Vector3 pos) {
    syncPos = pos;
  }
}
