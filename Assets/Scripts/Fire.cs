using UnityEngine;
using System.Collections;

public class Fire : TargetInteract{
	private bool active = true;


	public override string GetText(){
		if(active )
			return "Press E to extinguish fire";
		return "Press E to fire it up";
	}

	public override void Trigger(){
		active = !active;
		gameObject.SetActive(active);	
	}
}
