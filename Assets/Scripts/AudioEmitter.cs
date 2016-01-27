using UnityEngine;
using System.Collections;

public class AudioEmitter : MonoBehaviour {

	public SoundVision soundVision;

	// Use this for initialization
	void Start () {
		Debug.Log ("TEST");
	}

	void OnCollisionEnter(Collision collision)
	{
		Debug.Log ("SOUND");
		Debug.Log (collision.relativeVelocity.magnitude);
		soundVision.CreateSound (transform.position, collision.relativeVelocity.magnitude);
	}

	void onTriggerStay(Collider other)
	{
		Debug.Log ("SOUND");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
