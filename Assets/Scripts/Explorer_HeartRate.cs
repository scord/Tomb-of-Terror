using UnityEngine;
using System.Collections;
using System;

public class Explorer_HeartRate : MonoBehaviour {

  [SerializeField] private int HeartRate;
  [SerializeField] private AudioSource m_OpenValve;
  [SerializeField] private AudioSource m_CloseValve;
  private HeartRateManager HMR_Script;

  private HeartBeats heartBeatsScript;

  void Start() {
    //HMR_Script = (Instantiate (Resources.Load ("HeartRate")) as GameObject).GetComponent<HeartRateManager>();
    HMR_Script = GameObject.Find("HeartRate").GetComponent<HeartRateManager>();
    HMR_Script.EventHRUpdate += UpdateHeartRate;
    heartBeatsScript = new HeartBeats(m_OpenValve, m_CloseValve);
  }

  void Update() {
    heartBeatsScript.Beat(HeartRate);
  }

  void OnDisable() {
    if ( HMR_Script != null ) HMR_Script.EventHRUpdate -= UpdateHeartRate;
  }

  void UpdateHeartRate(int newHR) {
    HeartRate = newHR;
  }

  public int GetHeartBeat() {
    return HeartRate;
  }

}
