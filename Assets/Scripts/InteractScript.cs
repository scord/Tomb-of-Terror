using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractScript : MonoBehaviour {

	public TargetInteract targetObject;
	public bool withKey;

  private string m_TriggerKey = "Fire3";

	private string infoText;

	void Start(){
		// if(GameObject.Find("Explorer"))
			// info = GameObject.Find("Explorer/FireTip").GetComponent<Text>();
		// info.enabled = false;
	}

	public string PreInteract(){
		if(withKey){
			infoText = "Press X to " +targetObject.GetText();
			// info.enabled = true;
		}
		return infoText;
	}

	public void EndInteract(){
		// info.enabled = false;
	}

	public string Interact() {
		targetObject.Trigger();
		infoText = "Press X to " +targetObject.GetText();
		return infoText;
	}

	public string GetKeyCode() {
		return m_TriggerKey;
	}
}
