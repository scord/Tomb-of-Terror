using UnityEngine;
using System.Collections;
using System;

public class HeartBeats {

  [SerializeField] private AudioSource m_OpenValve;
  [SerializeField] private AudioSource m_CloseValve;

  private bool HRAudioSelect = false;
  private float heartBeatTimer = 0.0f;
  private float firstMarkSpace = 0.5F;
  private double heartVolumeExpScale = 1.035f;
  public int normalHR = 65;

  public HeartBeats(AudioSource open, AudioSource close) {
    m_OpenValve = open;
    m_CloseValve = close;
  }

	// Update is called once per frame
	public void Beat(int HeartRate) {
    heartBeatTimer += Time.deltaTime;
    if (HRAudioSelect){
      float delay = firstMarkSpace*60/((float)(HeartRate));

      if (heartBeatTimer > delay) {
          HRAudioSelect = !HRAudioSelect;
          m_OpenValve.pitch = 1.0f;
          m_OpenValve.volume = Mathf.Clamp((float)Math.Pow(((double)HeartRate - normalHR)/10.0f,heartVolumeExpScale),0,1); // higher heart rate is louder
          if(!m_OpenValve.isPlaying)
            m_OpenValve.Play();
          else{
            m_OpenValve.Stop();
            m_OpenValve.Play();
            Debug.Log("play");
          }
          heartBeatTimer = 0.0f;
      }
    } else {
      float delay = (1-firstMarkSpace)*60/((float)(HeartRate));

      if (heartBeatTimer > delay){
          //Debug.Log("close, Delay: "+delay+" time: "+((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)));
          HRAudioSelect = !HRAudioSelect;
          m_CloseValve.pitch = 1.0f;
          m_CloseValve.volume = Mathf.Clamp((float)Math.Pow(((double)HeartRate - normalHR)/10.0f,heartVolumeExpScale),0,1); // higher heart rate is louder
          if(!m_OpenValve.isPlaying)
            m_CloseValve.Play();
          else{
            m_CloseValve.Stop();
            m_CloseValve.Play();
            Debug.Log("play11");
          }

          heartBeatTimer = 0.0f;
      }
    }
	}
}
