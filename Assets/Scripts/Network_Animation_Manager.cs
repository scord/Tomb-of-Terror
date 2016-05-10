using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Network_Animation_Manager : NetworkBehaviour {

  [SyncVar] private bool isRunning = false;
  [SyncVar] private bool isMoving = false;
  [SerializeField] private Animator m_Animator;
  [SerializeField] private AudioSource m_FootStep1;
  [SerializeField] private AudioSource m_FootStep2;
  private bool lastRunValue;
  private bool lastMoveValue;

  bool leftFoot = true;
  // Use this for initialization
  void Start () {
    if(isLocalPlayer) {
      lastMoveValue = false;
      lastRunValue = false;
    }
  }

	// Update is called once per frame
	void Update () {
    if (!isLocalPlayer) {
      lastMoveValue = isMoving;
      lastRunValue = isRunning;
    } else {
      bool updateMoveValue = false;
      bool updateRunValue = false;
      if(Input.GetAxis("Vertical") != 0) {
        updateMoveValue = true;
      }
      if( Input.GetKey(KeyCode.LeftShift) || OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0 ) {
        updateRunValue = true;
      }
      SendAnimationState(updateRunValue, updateMoveValue);
      lastRunValue = updateRunValue;
      lastMoveValue = updateMoveValue;
    }
    PlayAnimations();
    PlayFootsteps();
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
        if ((isMoving || isRunning) && m_FootStep1.isPlaying == false && m_FootStep2.isPlaying == false)// add more logic later such as, onground/jumping etc etc
        {
            if (leftFoot)
            {
                m_FootStep1.pitch = Random.Range(0.7f, 0.9f);
                m_FootStep1.volume = Random.Range(0.7f, 0.9f);
                m_FootStep1.Play();
            }
            else
            {
                m_FootStep2.pitch = Random.Range(0.7f, 0.9f);
                m_FootStep2.volume = Random.Range(0.7f, 0.9f);
                m_FootStep2.Play();
            }
            leftFoot = !leftFoot;
        }
    }
}
