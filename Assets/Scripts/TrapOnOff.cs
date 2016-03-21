using UnityEngine;
using System.Collections;

public class TrapOnOff : MonoBehaviour {

	public GameObject door;

	void OnTriggerEnter(Collider other){
		Debug.Log("Entering");
		door.GetComponent<Rigidbody>().velocity = transform.up * -10;	
	}
}
