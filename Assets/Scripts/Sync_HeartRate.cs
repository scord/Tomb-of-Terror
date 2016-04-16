using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class Sync_HeartRate : NetworkBehaviour {
  [SyncVar (hook = "SyncHeartRate")] public int HeartRate;

  [SerializeField] private AudioSource m_OpenValve;
  [SerializeField] private AudioSource m_CloseValve;
  [SerializeField] private Explorer_HeartRate m_Explorer_HeartRate;

  private HeartBeats heartBeatsScript;

  void Start() {
    heartBeatsScript = new HeartBeats(m_OpenValve, m_CloseValve);
  }

  void Update() {
    if (isLocalPlayer) UpdateHeartRate(m_Explorer_HeartRate.GetHeartBeat());
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
  }

  [Client]
  void SyncHeartRate(int newHR) {
    HeartRate = newHR;
  }
}
