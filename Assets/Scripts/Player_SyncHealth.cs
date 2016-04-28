using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_SyncHealth : NetworkBehaviour {

  [SerializeField] [SyncVar (hook = "OnLivesUpdated")] private int m_Lives = 3; //lives
  [SerializeField] private Player_SyncPoints m_PlayerSyncPoints;
  private int localLives;

  public override void OnStartServer() {
    m_Lives = m_PlayerSyncPoints.necessaryPoints/m_PlayerSyncPoints.defaultValuePoints + 1;
  }

  void Start() {
    localLives = m_Lives;
  }


  [Server]
  public void Swipe() {
    m_Lives--;
  }

  [Client]
  void OnLivesUpdated(int newValue) {
    m_Lives = newValue;
    if (isLocalPlayer) {
      //Do things like run faster
      Debug.Log("I was swiped");
    }
  }
}
