using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractScript : MonoBehaviour {

	private Text info;
	public TargetInteract targetObject;
	// public GameObject handle;

	private string infoText;

	void Start(){
		info = GameObject.Find("Text").GetComponent<Text>();
		info.enabled = false;
	}

	public void PreInteract(){
		info.text = targetObject.GetText();
		info.enabled = true;
	}

	public void EndInteract(){
		info.enabled = false;
	}

	public void Interact() {
		targetObject.Trigger();
	}
}
