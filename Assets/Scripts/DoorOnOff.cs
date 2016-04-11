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
		Debug.Log("PLAY SOOOOUND");
		doorSound.Play();
	}

	void OnTriggerStay(Collider other) {
		if(doorBody.useGravity)
			doorBody.useGravity = false;
	}

	void Update(){
		if (doorBody.IsSleeping()){
			// Debug.Log("STOOOOP");
			// doorSound.Stop();
		}
	}

	void OnTriggerExit(Collider other){
		doorBody.useGravity = true;
		doorSound.Stop();
	}
}
