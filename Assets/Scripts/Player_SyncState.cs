using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player_SyncState : NetworkBehaviour {

  [SyncVar] [SerializeField] private int m_State;
  [SerializeField] private bool m_ShouldChangeOnTrigger;
  [SerializeField] private Player_SyncPoints m_PlayerSyncPoints;

  public bool carryingPrize;
    

  private const int state_end = 2;

  void Start() {
    carryingPrize = false;
    if (m_PlayerSyncPoints != null) m_PlayerSyncPoints.ChangeStateEvent += IncrementState; 
  }
	// Update is called once per frame
	void Update () {
    if (!isServer) return;
    if (m_State == state_end) GameObject.Find("NetworkManager").GetComponent<NetworkManagerCustom>().ManageFinalState(); 
	}

  void IncrementState() {
    if (isServer) m_State++;
    carryingPrize = (m_State == 1);
  }

  void OnTriggerEnter(Collider coll) {
    if ( m_ShouldChangeOnTrigger && isServer && ( coll.gameObject.tag == "PyramidExit")) {
      if ( state_end == m_State + 1) IncrementState();
    }
  }

  public bool GetCarryingPrize() {
    return (m_State == 1);
  }

  void OnDisable() {
    if (m_PlayerSyncPoints != null) m_PlayerSyncPoints.ChangeStateEvent -= IncrementState;
  }

}
