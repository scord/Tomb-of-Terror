using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player_SyncState : NetworkBehaviour {

  [SyncVar] [SerializeField] private int m_State;
  [SerializeField] private bool m_ShouldChangeOnTrigger;

    public bool carryingPrize;
    
  private Pickup_Manager m_PickupManager;

  private const int state_end = 2;

  void Start() {
        carryingPrize = false;
    m_PickupManager = GetComponent<Pickup_Manager>();
    if (m_PickupManager != null) m_PickupManager.ChangeStateEvent += IncrementState; 
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

  void OnDisable() {
    if (m_PickupManager != null) m_PickupManager.ChangeStateEvent -= IncrementState;
  }

}
