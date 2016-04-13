using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractScript : MonoBehaviour {

	public TargetInteract targetObject;
	public bool withKey;
	[SerializeField] private KeyCode m_TriggerKey = KeyCode.E;

	private string infoText;

	void Start(){
		// if(GameObject.Find("Explorer"))
			// info = GameObject.Find("Explorer/FireTip").GetComponent<Text>();
		// info.enabled = false;
	}

	public string PreInteract(){
		if(withKey){
			infoText = "Press " +  m_TriggerKey.ToString() + " to " +targetObject.GetText();
			// info.enabled = true;
		}
		return infoText;
	}

	public void EndInteract(){
		// info.enabled = false;
	}

	public string Interact() {
		targetObject.Trigger();
		infoText = "Press " +  m_TriggerKey.ToString() + " to " +targetObject.GetText();
		return infoText;
	}

	public KeyCode GetKeyCode() {
		return m_TriggerKey;
	}
}
