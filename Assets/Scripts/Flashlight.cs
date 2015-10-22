using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour {
	public GameObject player;
	private Vector3 offset;
	
	void Start(){
		offset = transform.position - player.transform.position;
	}
	
	void LateUpdate(){
		transform.position = player.transform.position + offset;
		Vector3 forward = transform.TransformDirection(Vector3.forward);
	}
}
