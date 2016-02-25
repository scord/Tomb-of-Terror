using UnityEngine;
using System.Collections;

public class DoorOnOff : MonoBehaviour {

	public GameObject door; 

	void Start(){
		Vector3 doorspeed = door.GetComponent<Rigidbody>().velocity = transform.up * 0;
	}

	void OnTriggerEnter(Collider other){
		door.GetComponent<Rigidbody>().velocity = transform.up * 3;	
	}

	void OnTriggerExit(Collider other){
		door.GetComponent<Rigidbody>().velocity = transform.up * -3;
	}

}
