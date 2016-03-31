using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Explorer_HeartRate : NetworkBehaviour {
  [SyncVar (hook = "SyncHeartRate")] public int HeartRate;
  [SerializeField] private AudioSource m_AudioSource;
  private HeartRateManager HMR_Script;

  private bool hasChanged = false;

  void Start() {
    if (isLocalPlayer) {
      HMR_Script = GameObject.Find("HeartRate").GetComponent<HeartRateManager>();
      HMR_Script.EventHRUpdate += UpdateHeartRate;
    }
  }

  void Update() {
    if (hasChanged) {
      hasChanged = false; 
      m_AudioSource.volume = (float) HeartRate/260.0f;
      m_AudioSource.Play();
    } 
    if ( !m_AudioSource.isPlaying ) {
      m_AudioSource.Play();
    }
  }

  void OnDisable() {
    if ( isLocalPlayer ) {
      HMR_Script.EventHRUpdate -= UpdateHeartRate;
    }
  }

  void UpdateHeartRate(int newHR) {
    if ( newHR != HeartRate ) CmdUpdateHeartRate(newHR);
  }

  [Command]
  void CmdUpdateHeartRate(int newHR) {
    HeartRate = newHR;
  }

  [Client]
  void SyncHeartRate(int newHR) {
    HeartRate = newHR;
    hasChanged = true;
  }
}
