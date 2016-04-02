using UnityEngine;
using System.Collections;

public class DoorOnOff : MonoBehaviour {

	public Door door;

	private Rigidbody doorBody;
	private AudioSource doorSound; 

	void Start(){
		 doorBody = door.GetComponent<Rigidbody>();
		 doorSound = door.GetComponent<AudioSource>();
		// AudioSource doorOffSound = door.GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider other){
		doorBody.useGravity = false;
		doorBody.velocity = transform.up * 3;	
		
		doorSound.clip = door.doorOn_sound;
		doorSound.time = 1;
		doorSound.Play();
	}

	void Update(){
		if (doorBody.IsSleeping()){
			doorSound.Stop();
		}
	}

	void OnTriggerExit(Collider other){
		doorBody.useGravity = true;
		doorSound.Stop();
	}
}
