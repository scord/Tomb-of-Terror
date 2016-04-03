using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class Explorer_HeartRate : NetworkBehaviour {
  [SyncVar (hook = "SyncHeartRate")] public int HeartRate;
  [SerializeField] private AudioSource m_AudioSource;

  [SerializeField] private AudioSource m_OpenValve;
  [SerializeField] private AudioSource m_CloseValve;
  private HeartRateManager HMR_Script;
  private bool HRAudioSelect = false;
  private float heartBeatTimer = 0.0f;
  private float firstMarkSpace = 0.5F;
  private double heartVolumeExpScale = 1.035f;
  public int normalHR = 65;

  private bool hasChanged = false;

  void Start() {
    if (isLocalPlayer) {
      HMR_Script = GameObject.Find("HeartRate").GetComponent<HeartRateManager>();
      HMR_Script.EventHRUpdate += UpdateHeartRate;
    }
  }

  void Update() {
    heartBeatTimer += Time.deltaTime;
    if (HRAudioSelect){
        float delay = firstMarkSpace*60/((float)(HeartRate));
        
        if (heartBeatTimer > delay){
            //Debug.Log("open, Delay: "+delay+" time: "+((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)));
            HRAudioSelect = !HRAudioSelect;
            m_OpenValve.pitch = 1.0f;
            m_OpenValve.volume = (float)Math.Pow((double)HeartRate,heartVolumeExpScale)/normalHR; //high heart rate is louder
            m_OpenValve.Play();
            heartBeatTimer = 0.0f;
        }
        
    } else {
        
        float delay = (1-firstMarkSpace)*60/((float)(HeartRate));
        
        if (heartBeatTimer > delay){
            //Debug.Log("close, Delay: "+delay+" time: "+((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)));
            HRAudioSelect = !HRAudioSelect;
            m_CloseValve.pitch = 1.0f;
            m_CloseValve.volume = (float)Math.Pow((double)HeartRate,heartVolumeExpScale)/normalHR; // higher heart rate is louder
            m_CloseValve.Play();
            
            heartBeatTimer = 0.0f;
        }
      }
    /*if (hasChanged) {
      hasChanged = false; 
      m_AudioSource.volume = 0; //(float) HeartRate/260.0f;
      m_AudioSource.Play();
    } 
    if ( !m_AudioSource.isPlaying ) {
      m_AudioSource.Play();
    }*/
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
