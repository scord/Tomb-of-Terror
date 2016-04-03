using UnityEngine;
using System.Collections;
using System;

public class CentreHeartBeat : MonoBehaviour {


    private float firstMarkSpace = 0.5F;
    private double heartVolumeExpScale = 1.035f;
    private HeartRateManager HRManager;
    
    private float heartBeatTimer;
    private bool HRAudioSelect;
    private int HR;
    public int normalHR = 65;
    public AudioSource openValve;
    public AudioSource closeValve;

	// Use this for initialization
	void Start () {
        HRAudioSelect = true;
	    heartBeatTimer = 0.0f;
        Debug.Log("Global Start");
        HRManager = GameObject.Find("HeartRate").GetComponent<HeartRateManager>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Global Updates");
        heartBeatTimer += Time.deltaTime;
        
        
        HR = HRManager.HeartRate;


        if (HRAudioSelect){
            float delay = firstMarkSpace*60/((float)(HR));
            
            if (heartBeatTimer > delay){
                //Debug.Log("open, Delay: "+delay+" time: "+((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)));
                HRAudioSelect = !HRAudioSelect;
                openValve.pitch = 1.0f;
                openValve.volume = (float)Math.Pow((double)HR,heartVolumeExpScale)/normalHR; //high heart rate is louder
                openValve.Play();
                Debug.Log("Global working O");
                heartBeatTimer = 0.0f;
            }
            
        } else {
            
            float delay = (1-firstMarkSpace)*60/((float)(HR));
            
            if (heartBeatTimer > delay){
                //Debug.Log("close, Delay: "+delay+" time: "+((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)));
                HRAudioSelect = !HRAudioSelect;
                closeValve.pitch = 1.0f;
                closeValve.volume = (float)Math.Pow((double)HR,heartVolumeExpScale)/normalHR; // higher heart rate is louder
                closeValve.Play();
                Debug.Log("Global working C");
                heartBeatTimer = 0.0f;
            }
            

        }
	}
}
