using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Explorer_SyncAir : NetworkBehaviour {

  [SyncVar] public int m_AirLevel;
  private AirManager m_AirManager;
	private GameObject m_ForTimer;
	public Text m_ForTimerText;

	// Use this for initialization
	void Start () {
    if ( isServer ) {
      GameObject go = GameObject.Find("AirManager");
      if (go != null) {
        m_AirManager = go.GetComponent<AirManager>();
        m_AirManager.EventAirUpdate += UpdateAir;
      }
    }
//		if (isLocalPlayer) {
//			GameObject m_ForTimer = GameObject.Find ("Timer");
//			if (m_ForTimer != null)
//				m_ForTimerText = m_ForTimer.GetComponent<Text> ();
//		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) return;
		if (m_ForTimerText == null) return;
		if ( (m_AirLevel / 60).ToString() + ":" + (m_AirLevel % 60).ToString() != m_ForTimerText.text) {
			int min, sec;
			min = m_AirLevel / 60;
			sec = m_AirLevel % 60;
      if(sec < 10)
			 m_ForTimerText.text = min.ToString() + ":0" + sec.ToString();
      else
        m_ForTimerText.text = min.ToString() + ":" + sec.ToString();
		}
			
	}

  void OnDisable() {
    if (m_AirManager != null) m_AirManager.EventAirUpdate -= UpdateAir;
  }

  void UpdateAir(int newAir) {
    if ( (m_AirLevel - newAir >= 1) || ( m_AirLevel < newAir )) m_AirLevel = newAir;
  }
}
