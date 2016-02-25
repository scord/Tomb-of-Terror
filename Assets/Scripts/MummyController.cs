using UnityEngine;
using System.Collections;

public class MummyController : PlayerController {
	
	
	protected override void Start(){
		base.Start();
	}	


	protected override void Update(){
		base.Update();
	    if (Input.GetButton("Fire1")){
            gameObject.GetComponent<AudioSource>().Play();
            soundVision.EchoLocate();
        }

        if (Input.GetButton("Fire2"))
            if (!carrying)
                PickUp();
            else
                Throw();
	}
}