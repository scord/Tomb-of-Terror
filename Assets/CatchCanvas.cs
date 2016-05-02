using UnityEngine;
using System.Collections;

public class CatchCanvas : MonoBehaviour {

	bool finished = false;
	private MummyIntroScript canvas;

	public void OnTriggerStay(Collider mummyCollider){
		if(mummyCollider.tag == "Player"){
			finished =  mummyCollider.gameObject.GetComponent<MummyController>().CheckTutorial();
			Debug.Log("works");
			if(finished){
				Debug.Log("trigger run ");
				canvas = mummyCollider.gameObject.transform.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor/OVRTrackerBounds/Canvas").gameObject.GetComponent<MummyIntroScript>();
				canvas.Catch();
				Destroy(gameObject);
			}
		}
	}
}
