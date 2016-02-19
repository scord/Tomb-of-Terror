using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public Camera cam;
	public SoundVision soundVision;
    public AudioSource audio_source;
    GameObject carriedObject;
    bool carrying = false;
    bool turned;
	// public SoundVision test;
    // Use this for initialization
    void Start () {
	    turned = false;

  
       audio_source.clip = (AudioClip)Resources.Load("AudioClips/Footstep1");
    }
	
	// Update is called once per frame
	void Update () {
        
        float moveVertical = Input.GetAxis("Vertical");
        float lookHorizontal = Input.GetAxis("RightH");
        float lookVertical = Input.GetAxis("RightV");

        if (moveVertical == 1.0)
        {
            transform.position = transform.position + cam.transform.forward * 2 * Time.deltaTime;
            if (audio_source.isPlaying == false)// add more logic later such as, onground/jumping etc etc
            {
               // AudioSource.PlayClipAtPoint(footstep_Sound1, transform.position);
               // footstep_playing = 1;
                audio_source.pitch = Random.Range(0.8f, 1);
                audio_source.volume = Random.Range(0.8f, 1.1f);
                audio_source.Play();
            }
        }

        if (Input.GetButton("Fire1"))
        {
            gameObject.GetComponent<AudioSource>().Play();
            soundVision.EchoLocate();
        }
        if (Input.GetButton("Fire2"))
            if (!carrying)
                PickUp();
            else
                Throw();

        if ((lookHorizontal == 1.0 || lookHorizontal == -1.0) && turned == false)
        {
            transform.Rotate(lookHorizontal * Vector3.up);
       //     turned = true;
        } /*else if (moveHorizontal != 1.0 && moveHorizontal != -1.0)
        {
            turned = false;
        }*/
        if ((lookVertical == 1.0 || lookVertical == -1.0) && turned == false)
        {
            
        }

            if (carrying)
        {
            carriedObject.transform.position = cam.transform.position + cam.transform.TransformDirection(Vector3.forward)*2;
        }

    }

    void Throw()
    {
        carriedObject.GetComponent<Rigidbody>().isKinematic = false;
        carrying = false;
        carriedObject.GetComponent<Rigidbody>().AddForce(cam.transform.TransformDirection(Vector3.forward) * 100);
    }

    void PickUp()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 4))
        {
            if (hit.collider.gameObject.tag == "PickUp")
            {
                carriedObject = hit.collider.gameObject;
                carriedObject.GetComponent<Rigidbody>().isKinematic = true;
                carrying = true;
            }
        }
    }

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Explorer") {
			Debug.Log("MUMMY WINS");
		}
	}
}
