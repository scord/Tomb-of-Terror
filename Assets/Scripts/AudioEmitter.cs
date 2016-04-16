using UnityEngine;
using System.Collections;

public class AudioEmitter : MonoBehaviour {

	public SoundVision soundVision;
	private AudioSource sound;
	private Rigidbody objBody;
	// Use this for initialization
	void Start () {
		objBody = gameObject.GetComponent<Rigidbody>();
		sound = gameObject.GetComponent<AudioSource>();
	}

	void OnCollisionEnter(Collision collision)
	{
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(0.8f, 1);
        audioSource.volume = Random.Range(collision.relativeVelocity.magnitude/5 - 0.1f, collision.relativeVelocity.magnitude/5);
        sound.Play();
	}

	void OnCollisionStay(Collision collision){
		if(objBody.IsSleeping()){
			sound.Stop();
		}
	}

	void onTriggerStay(Collider other){
	}

	// Update is called once per frame
	void Update () {
	}
}
