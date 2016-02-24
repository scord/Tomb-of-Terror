using UnityEngine;
using System.Collections;

public class FireOnOff : MonoBehaviour {

	public GameObject torch; 

	void OnTriggerExit(Collider other){
		torch.active = false;
	}

}
