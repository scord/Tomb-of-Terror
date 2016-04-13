using UnityEngine;
using System.Collections;

public class AudioEmitter : MonoBehaviour {

	public SoundVision soundVision;

	// Use this for initialization
	void Start () {

	}

	void OnCollisionEnter(Collision collision)
	{
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(0.8f, 1);
        Debug.Log("TEST");
        audioSource.volume = Random.Range(collision.relativeVelocity.magnitude/5 - 0.1f, collision.relativeVelocity.magnitude/5);
        gameObject.GetComponent<AudioSource>().Play();
	}

	void onTriggerStay(Collider other){
	}

	// Update is called once per frame
	void Update () {
	}
}
