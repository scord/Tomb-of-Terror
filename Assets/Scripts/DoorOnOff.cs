using UnityEngine;
using System.Collections;

public class DoorOnOff : MonoBehaviour {

	public GameObject door; 
	private	float speed = 20f;
	private Vector3 openDoor = new Vector3(0, 10, 0);
	private Vector3 doorFinalPos ;

	void Start(){
		doorFinalPos = door.transform.localPosition + openDoor;
	}

	void OnTriggerEnter(Collider other){
     	door.transform.localPosition = Vector3.MoveTowards(door.transform.localPosition, doorFinalPos, 10f * Time.deltaTime);
		Debug.Log("trigger");
		// door.transform.Translate(openDoor * 1);
	}
}
