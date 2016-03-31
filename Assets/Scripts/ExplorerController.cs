using UnityEngine;
using System.Collections;

public class ExplorerController : PlayerController {
	
	//private Light torchIntensity;


	//private float wheelDirection;


	protected override void Start(){    
		base.Start();
		//torchIntensity = GetComponentsInChildren<Light>()[0];
	}
	
	protected override void Update(){
		base.Update();
		//wheelDirection = Input.GetAxis("Mouse ScrollWheel");
      //  if (wheelDirection > 0)
      //      torchIntensity.intensity += 0.20f;
      //  else if (wheelDirection <  0)
      //          torchIntensity.intensity -= 0.20f;

        if (Input.GetButtonDown("Fire2"))
            if (!carrying)
                PickUp();
            else
                Throw();


        // deal with in-game interactions
        /*if (onTrigger && trig != null && trig.withKey){
            if(Input.GetKeyDown(m_TriggerKey)){
                trig.Interact();
            }
        }*/
	}

}