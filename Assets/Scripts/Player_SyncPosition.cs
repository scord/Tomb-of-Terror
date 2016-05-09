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
            //Debug.Log("pos1 " + m_Transform.position + " pos2 " + syncPos);
            if ( m_Transform.position == syncPos ) {
        //m_Animator.SetBool("Movement", false);
      } else {
        //m_Animator.SetBool("Movement", true);
        m_Transform.position = Vector3.Lerp(m_Transform.position, syncPos, Time.deltaTime * lerpRate);



      }
		} else {
		float moveVertical = Input.GetAxis("Vertical");
            //float lookHorizontal = Input.GetAxis("RightH");
            //float lookVertical = Input.GetAxis("RightV");
            Debug.Log("moveV " + moveVertical);
            if (moveVertical == 1.0 || moveVertical == -1.0) {

				bool move = true;
				if (footStep1.isPlaying == false && footStep2.isPlaying == false) {// add more logic later such as, onground/jumping etc etc
					if (leftFoot) {
						// AudioSource.PlayClipAtPoint(footstep_Sound1, transform.position);
						// footstep_playing = 1;
						footStep1.pitch = Random.Range (0.7f, 0.9f);
						footStep1.volume = Random.Range (0.7f, 0.9f);
						footStep1.Play ();
					} else {
						footStep2.pitch = Random.Range (0.7f, 0.9f);
						footStep2.volume = Random.Range (0.7f, 0.9f);
						footStep2.Play ();
					}
					leftFoot = !leftFoot;
				}
			}
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
