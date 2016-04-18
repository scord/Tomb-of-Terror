using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Explorer_SyncAir : NetworkBehaviour {

  [SyncVar] public int m_AirLevel;
  private AirManager m_AirManager;

	// Use this for initialization
	void Start () {
    if ( isServer ) {
      GameObject go = GameObject.Find("AirManager");
      if (go != null) {
        m_AirManager = go.GetComponent<AirManager>();
        m_AirManager.EventAirUpdate += UpdateAir;
      }
    }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  void OnDisable() {
    if (m_AirManager != null) m_AirManager.EventAirUpdate -= UpdateAir;
  }

  void UpdateAir(int newAir) {
    if ( (m_AirLevel - newAir >= 1) || ( m_AirLevel < newAir )) m_AirLevel = newAir;
  }
}
