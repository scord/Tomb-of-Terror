using UnityEngine;
using System.Collections;

public class ExplorerController : PlayerController {
	
	private Light torchIntensity;
	private InteractScript trig;

	private float wheelDirection;
	private bool onTrigger;


	protected override void Start(){
        GetComponent<IKHandler>().enabled = true;
		base.Start();
		onTrigger = false;
		//torchIntensity = GetComponentsInChildren<Light>()[0];
	}
	
	protected override void Update(){
		base.Update();
		wheelDirection = Input.GetAxis("Mouse ScrollWheel");
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