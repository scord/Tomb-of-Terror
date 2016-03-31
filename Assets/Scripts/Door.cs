using UnityEngine;
using System.Collections;

public class Door : TargetInteract{
	private int direction = 1;
	private bool gravity = true;

	public void Start(){
		GetComponent<Rigidbody> ().useGravity = gravity;
	}

	public override string GetText(){
		return "Press E to open Door";
	}

	public override void Trigger(){
		gravity = !gravity;	
		GetComponent<Rigidbody>().useGravity = false;
		Debug.Log(gravity);
		Debug.Log("I am here !!!!");
		Debug.Log(GetComponent<Rigidbody>().useGravity);

		GetComponent<Rigidbody>().velocity = transform.up * 3 * direction;	
		direction = -direction;
	}
}
