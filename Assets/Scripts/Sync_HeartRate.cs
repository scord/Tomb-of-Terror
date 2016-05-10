using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class Sync_HeartRate : NetworkBehaviour {
  [SyncVar (hook = "SyncHeartRate")] public int HeartRate;

  [SerializeField] private AudioSource m_OpenValve;
  [SerializeField] private AudioSource m_CloseValve;
  [SerializeField] private Explorer_HeartRate m_Explorer_HeartRate;
  [SerializeField] private UnityEngine.UI.Text m_HeartRateText;

  private HeartBeats heartBeatsScript;

  void Start() {
    heartBeatsScript = new HeartBeats(m_OpenValve, m_CloseValve);
  }

  void Update() {
    if (isLocalPlayer) {
      if ( Input.GetKeyDown(KeyCode.P) ) {
        m_HeartRateText.gameObject.SetActive(!m_HeartRateText.gameObject.activeSelf);
      }
      UpdateHeartRate(m_Explorer_HeartRate.GetHeartBeat());
    }
  }

  void FixedUpdate() {
    if (!isLocalPlayer) heartBeatsScript.Beat(HeartRate);
  }

  void UpdateHeartRate(int newHR) {
    if ( newHR != HeartRate ) CmdUpdateHeartRate(newHR);
  }

  [Command]
  void CmdUpdateHeartRate(int newHR) {
    HeartRate = newHR;
    m_HeartRateText.text = HeartRate.ToString();
  }

  [Client]
  void SyncHeartRate(int newHR) {
    HeartRate = newHR;
  }
}
