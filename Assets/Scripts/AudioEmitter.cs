using UnityEngine;
using System.Collections;

public class AudioEmitter : MonoBehaviour {

	public SoundVision soundVision;

	// Use this for initialization
	void Start () {
		Debug.Log ("TEST");
	}

	void OnCollisionEnter(Collision other)
	{
		Debug.Log ("SOUND");
		soundVision.CreateSound (transform.position);
	}

	void onTriggerStay(Collider other)
	{
		Debug.Log ("SOUND");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
