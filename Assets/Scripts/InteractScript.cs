using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractScript : MonoBehaviour {

	private Text info;
	public TargetInteract targetObject;
	public bool withKey;
	[SerializeField] private KeyCode m_TriggerKey = KeyCode.E;

	private string infoText;

	void Start(){
		//info = GameObject.Find("Text").GetComponent<Text>();
		//info.enabled = false;
		Debug.Log("Interact Started");
	}

	public void PreInteract(){
		if(withKey){
			//info.text = "Press " +  m_TriggerKey.ToString() + " to " +targetObject.GetText();
			//info.enabled = true;
		}
	}

	public void EndInteract(){
		//info.enabled = false;
	}

	public void Interact() {
		targetObject.Trigger();
	}

	public KeyCode GetKeyCode() {
		return m_TriggerKey;
	}
}