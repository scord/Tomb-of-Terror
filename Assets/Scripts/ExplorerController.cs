using UnityEngine;
using System.Collections;
using System;

public class ExplorerController : PlayerController {
	
	private Light torchIntensity;
	private InteractScript trig;


	private float wheelDirection;
	private bool onTrigger;

    private float firstMarkSpace = 0.5F;
    private double heartVolumeExpScale = 1.035f;
    private HeartRateManager HRManager;
    
    private float heartBeatTimer;
    private bool HRAudioSelect;
    private int HR;
    public int normalHR = 65;
    public AudioSource openValve;
    public AudioSource closeValve;
    

	protected override void Start(){
        
        HRAudioSelect = true;
        
		base.Start();
		onTrigger = false;
		//torchIntensity = GetComponentsInChildren<Light>()[0];

        heartBeatTimer = 0.0f;
        
        HRManager = GameObject.Find("HeartRate").GetComponent<HeartRateManager>();;
        
	}
	
	protected override void Update(){
		base.Update();
        
        heartBeatTimer += Time.deltaTime;
        
		wheelDirection = Input.GetAxis("Mouse ScrollWheel");
      //  if (wheelDirection > 0)
      //      torchIntensity.intensity += 0.20f;
      //  else if (wheelDirection <  0)
      //          torchIntensity.intensity -= 0.20f;
  
        HR = HRManager.HeartRate;


        if (HRAudioSelect){
            float delay = firstMarkSpace*60/((float)(HR));
            
            if (heartBeatTimer > delay){
                //Debug.Log("open, Delay: "+delay+" time: "+((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)));
                HRAudioSelect = !HRAudioSelect;
                openValve.pitch = 1.0f;
                openValve.volume = (float)Math.Pow((double)HR,heartVolumeExpScale)/normalHR; //high heart rate is louder
                openValve.Play();
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
                
                heartBeatTimer = 0.0f;
            }
            

        }
  
        if (Input.GetButtonDown("Fire2"))
            if (!carrying)
                PickUp();
            else
                Throw();


        // deal with in-game interactions
        if (onTrigger && trig.withKey){
            if(Input.GetKeyDown(KeyCode.E)){
                trig.Interact();
            }
        }
	}


    void OnTriggerEnter(Collider other){
        if(other.tag.Equals("Interaction")){
            trig = (InteractScript) other.GetComponent(typeof(InteractScript));
            onTrigger = true;
            if(trig.withKey)
                trig.PreInteract();
            else trig.Interact();
        }
    }
    
    void OnTriggerStay(Collider other){
        if(other.tag.Equals("Interaction") && !onTrigger){
            onTrigger = true;
            trig.PreInteract(); 
        }
    }

    void OnTriggerExit(Collider other){
        if(other.tag.Equals("Interaction")){
            onTrigger =false;
            trig.EndInteract();
        }
    }
}