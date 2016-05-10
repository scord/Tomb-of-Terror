using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_SyncHealth : NetworkBehaviour {

  [SyncVar (hook = "OnLivesUpdated")] private int m_Lives = 3; //lives
  [SerializeField] private Player_SyncPoints m_PlayerSyncPoints;
  [SerializeField] private VibrationController m_VibrationController;
  [SerializeField] private GameObject m_RedScreenCanvas;
  private OVRPlayerController m_OVRPlayerController;
  private int localLives;
	[SerializeField] private Text m_LivesText;
  [SerializeField] private AudioClip swipe_sound;
  public int lives {get{ return m_Lives;}}
  
  void Start() {
    if (isLocalPlayer) {
      CmdSyncLives();
      m_OVRPlayerController = gameObject.GetComponent<OVRPlayerController>();
    }
  }

  [Command]
  void CmdSyncLives() {
    m_Lives = m_Lives++;
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
      m_LivesText.text = m_Lives.ToString();
    }
  }

  [ClientRpc]
  public void RpcSendSwipeReaction() {
    if (isLocalPlayer) {
      m_VibrationController.VibrateFor(1.0f);
      m_LivesText.text = m_Lives.ToString ();
      MultiplyRunningSpeed(2.5f);
      StartCoroutine(RelaxSpeed());
			if (m_RedScreenCanvas != null){
				ScreenFlashController sfc = m_RedScreenCanvas.GetComponent<ScreenFlashController>();
				sfc.hit = true;
			}

      //Do things like run faster
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
