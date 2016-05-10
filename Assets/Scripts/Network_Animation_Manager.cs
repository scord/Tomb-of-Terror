using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Network_Animation_Manager : NetworkBehaviour {

  [SyncVar] private bool isRunning = false;
  [SyncVar] private bool isMoving = false;
  [SerializeField] private Animator m_Animator;
  private bool lastRunValue;
  private bool lastMoveValue;

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
}
