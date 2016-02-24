using UnityEngine;
using System.Collections;

public class Door : TargetInteract{
	private int direction = 1;

	public override string GetText(){
		return "Press E to open Door";
	}

	public override void Trigger(){
		GetComponent<Rigidbody>().velocity = transform.up * 3 * direction;	
		direction = -direction;
	}
}
