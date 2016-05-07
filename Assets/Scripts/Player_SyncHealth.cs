using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_SyncHealth : NetworkBehaviour {

  [SyncVar (hook = "OnLivesUpdated")] public int m_Lives = 3; //lives
  [SerializeField] private Player_SyncPoints m_PlayerSyncPoints;
  private OVRPlayerController m_OVRPlayerController;
  private int localLives;
	public Text lives;
    private AudioClip swipe_sound;
  void Start() {
        //localLives = m_Lives;
        swipe_sound = (AudioClip)Resources.Load("AudioClips/Mummy_swipe");
        if (isLocalPlayer) {
      CmdSyncLives();
      m_OVRPlayerController = gameObject.GetComponent<OVRPlayerController>();
    }
  }

  [Command]
  void CmdSyncLives() {
    Debug.Log("First: " + m_Lives);
    m_Lives = m_Lives++;
    Debug.Log("Second: " + m_Lives);
  }

  [Server]
  public void Swipe() {
    m_Lives--;
    RpcSendSwipeReaction();
  }


  [Client]
  void OnLivesUpdated(int newValue) {
    m_Lives = newValue;
    if(isLocalPlayer) {
      lives.text = m_Lives.ToString();
    }
  }

  [ClientRpc]
  public void RpcSendSwipeReaction() {
    Debug.Log("I get here");
    if (isLocalPlayer) {

      lives.text = m_Lives.ToString ();
      MultiplyRunningSpeed(6.0f);
      StartCoroutine(RelaxSpeed());
      //Do things like run faster
      Debug.Log("I was swiped");
            AudioSource.PlayClipAtPoint(swipe_sound, transform.position);
        }
  }

  IEnumerator RelaxSpeed() {
    yield return new WaitForSeconds(3.0f);
    MultiplyRunningSpeed(1.0f);
  }

  void MultiplyRunningSpeed(float scalingFactor) {
    if( m_OVRPlayerController != null ) m_OVRPlayerController.SetMoveScaleMultiplier(scalingFactor);
  }
}
