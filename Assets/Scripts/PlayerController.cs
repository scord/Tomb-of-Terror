using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public Camera cam;
	public SoundVision soundVision;
    public AudioSource audio_source;

    bool turned;
	// public SoundVision test;
    // Use this for initialization
    void Start () {
	    turned = false;

        audio_source = GetComponent<AudioSource>();
        audio_source.clip = (AudioClip)Resources.Load("AudioClips/Footstep1");
    }
	
	// Update is called once per frame
	void Update () {
        
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (moveVertical == 1.0)
        {
            transform.position = transform.position + cam.transform.forward * 2 * Time.deltaTime;
            if (audio_source.isPlaying == false)// add more logic later such as, onground/jumping etc etc
            {
                //AudioSource.PlayClipAtPoint(footstep_Sound1, transform.position);
                //footstep_playing = 1;
                audio_source.pitch = Random.Range(0.8f, 1);
                audio_source.volume = Random.Range(0.8f, 1.1f);
                audio_source.Play();
            }
        }

        if ((moveHorizontal == 1.0 || moveHorizontal == -1.0) && turned == false)
        {
            transform.Rotate(new Vector3(0.0f, moveHorizontal * 30, 0.0f));
            turned = true;
        } else if (moveHorizontal != 1.0 && moveHorizontal != -1.0)
        {
            turned = false;
        }
    }

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Explorer") {
			Debug.Log("MUMMY WINS");
		}
	}
}
