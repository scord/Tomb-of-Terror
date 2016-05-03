using UnityEngine;
using System.Collections;

public class runCanvas : MonoBehaviour {
	bool finished = false;
	private MummyIntroScript canvas;

	public void OnTriggerStay(Collider mummyCollider){
		if(mummyCollider.tag == "Player"){
			finished =  mummyCollider.gameObject.GetComponent<MummyController>().CheckTutorial();
			Debug.Log("tutorial" + finished);
			if(finished){
				canvas = mummyCollider.gameObject.transform.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor/OVRTrackerBounds/Canvas").gameObject.GetComponent<MummyIntroScript>();
				canvas.Run();
				Destroy(gameObject);
			}
		}
	}
}
