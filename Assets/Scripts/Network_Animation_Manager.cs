using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Network_Animation_Manager : NetworkBehaviour {

  [SyncVar] private bool isRunning = false;
  [SyncVar] private bool isMoving = false;
  [SerializeField] private Animator m_Animator;
  private bool lastRunValue;
  private bool lastMoveValue;

    public AudioSource footStep1;
    public AudioSource footStep2;
    bool leftFoot = true;
    // Use this for initialization
    void Start () {
	 if(isLocalPlayer) {
    lastMoveValue = false;
    lastRunValue = false;
   }
	}

  void FixedUpdate() {
    if (isLocalPlayer) {
      bool updateMoveValue = false;
      bool updateRunValue = false;
      if(Input.GetAxis("Vertical") != 0) {
        updateMoveValue = true;
      }
      if( Input.GetKey(KeyCode.LeftShift) || OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0 ) {
        updateRunValue = true;
      }
      SendAnimationState(updateRunValue, updateMoveValue);
      PlayAnimations();
      lastRunValue = updateRunValue;
      lastMoveValue = updateMoveValue;
    }
  }
	// Update is called once per frame
	void Update () {
    if (!isLocalPlayer) {
      lastMoveValue = isMoving;
      lastRunValue = isRunning;
      PlayAnimations();
      PlayFootsteps();
    }
	}

  void PlayAnimations() {
    m_Animator.SetBool("Run", isRunning);
    m_Animator.SetBool("Movement", isMoving);
  }

  [ClientCallback]
  void SendAnimationState(bool newRunValue, bool newMoveValue) {
    if ( isLocalPlayer && ((newMoveValue != lastMoveValue) || (newRunValue != lastRunValue))) {
      CmdSetAnimations(newRunValue, newMoveValue);
    }
  }

  [Command]
  void CmdSetAnimations(bool newRunValue, bool newMoveValue) {
    isRunning = newRunValue;
    isMoving = newMoveValue;
  }

  void PlayFootsteps()
    {
        if ((isMoving || isRunning) && footStep1.isPlaying == false && footStep2.isPlaying == false)// add more logic later such as, onground/jumping etc etc
        {
            if (leftFoot)
            {
                // AudioSource.PlayClipAtPoint(footstep_Sound1, transform.position);
                // footstep_playing = 1;
                footStep1.pitch = Random.Range(0.7f, 0.9f);
                footStep1.volume = Random.Range(0.7f, 0.9f);
                footStep1.Play();
            }
            else
            {
                footStep2.pitch = Random.Range(0.7f, 0.9f);
                footStep2.volume = Random.Range(0.7f, 0.9f);
                footStep2.Play();
            }
            leftFoot = !leftFoot;
        }
    }
}
