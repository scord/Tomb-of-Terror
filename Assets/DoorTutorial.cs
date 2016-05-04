using UnityEngine;
using System.Collections;

public class DoorTutorial : MonoBehaviour {
	[SerializeField] private GameObject doorBlock;
	[SerializeField] private GameObject doorTrigger;
	private bool finished = false;

	public void Start(){
		doorTrigger.SetActive(false);
	}

	public void OnTriggerStay(Collider mummyCollider){
		if(mummyCollider.tag == "Player"){
			finished =  mummyCollider.gameObject.GetComponent<MummyController>().CheckTutorial();
			Debug.Log("works");
			if(finished){
				Debug.Log("Open Door ");
				// door.GetComponent<Rigidbody>().velocity = transform.up * 2;
				doorTrigger.SetActive(true);
				// Destroy(doorBlock);
				Destroy(gameObject);
			}
		}
	}
}
