using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_SyncHealth : NetworkBehaviour {

  [SerializeField] [SyncVar (hook = "OnLivesUpdated")] private int m_Lives = 3; //lives
  [SerializeField] private Player_SyncPoints m_PlayerSyncPoints;
  private OVRPlayerController m_OVRPlayerController;
  private int localLives;
	public Text lives; 

  public override void OnStartServer() {
    //m_Lives = m_PlayerSyncPoints.necessaryPoints/m_PlayerSyncPoints.defaultValuePoints + 1;
  }

  void Start() {
    localLives = m_Lives;
		lives.text = m_Lives.ToString ();
    if (isLocalPlayer) {
      m_OVRPlayerController = gameObject.GetComponent<OVRPlayerController>();
    }
  }


  [Server]
  public void Swipe() {
    m_Lives--;
  }

  [Client]
  void OnLivesUpdated(int newValue) {
    m_Lives = newValue;
    if (isLocalPlayer) {
			lives.text = m_Lives.ToString ();
      MultiplyRunningSpeed(6.0f);
      StartCoroutine(RelaxSpeed());
      //Do things like run faster
      Debug.Log("I was swiped");
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
