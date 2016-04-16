using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExplorerInfo : TriggeredObject {
 public override void Trigger(Collider mummyCollider){
	 GameObject[] rocks = GameObject.FindGameObjectsWithTag("PickUp");
	 for(int i=0; i<rocks.Length; i++){
		 rocks[i].GetComponent<Rigidbody>().useGravity = true;
	 }
 }
}
