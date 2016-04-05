using UnityEngine;
using System.Collections;
using System;

public class MummyController : PlayerController {
	
	private float murmurTimer;
    
    private AudioSource shout;
    
	protected override void Start(){
		base.Start();
        
        shout = gameObject.GetComponent<AudioSource>();
        
        murmurTimer = 0.0f;
        
	}	

	protected override void Update(){
        
        murmurTimer += Time.deltaTime;
        
		base.Update();
	    if (Input.GetButtonDown("Fire1")){
            shout.volume = 2.0f;
            shout.Play();
            soundVision.EchoLocate();
            
            murmurTimer = 0.0f;
            
        }
        
        if (murmurTimer > 3.0f){
            
            shout.volume = 0.25f;
            shout.Play();
            
            murmurTimer = 0.0f;
        }
	}
}