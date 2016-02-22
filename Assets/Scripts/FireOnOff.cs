using UnityEngine;
using System.Collections;

public class FireOnOff : MonoBehaviour {

	public GameObject torch; 
	public GameObject trigger;

	void OnTriggerExit(Collider other){
		torch.active = false;
	}

	// void OnTriggerStay(Collider other){
	// 	torch.active = false;
	// }

	// void OnTriggerExit(Collider other){
	// 	torch.active = true;
	// }
}
