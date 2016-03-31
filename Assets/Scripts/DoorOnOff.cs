using UnityEngine;
using System.Collections;

public class DoorOnOff : MonoBehaviour {

	public GameObject door; 

	private	float speed = 20f;
	private Vector3 openDoor = new Vector3(0, 10, 0);
	private Vector3 doorFinalPos ;
	private Vector3 initialPos;
	public bool open;
	public bool closing = false;

	void Start(){
		Vector3 doorspeed = door.GetComponent<Rigidbody>().velocity = transform.up * 0;
	}

	void OnTriggerEnter(Collider other){
		door.GetComponent<Rigidbody> ().useGravity = false;
		door.GetComponent<Rigidbody>().velocity = transform.up * 3;	
	}

	void OnTriggerExit(Collider other){
		door.GetComponent<Rigidbody> ().useGravity = true;
		// door.GetComponent<Rigidbody>().velocity = transform.up * -3;
	}

}
