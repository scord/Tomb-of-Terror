using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public Camera cam;
	public SoundVision soundVision;
    public AudioSource audio_source;
    GameObject carriedObject;
    protected bool carrying;
    protected bool turned;
    protected bool move;
	// public SoundVision test;
    // Use this for initialization

    public Animator animator;

    virtual protected void Start () {
	    turned = false;
        carrying = false;
       animator = GetComponent<Animator>();
       audio_source.clip = (AudioClip)Resources.Load("AudioClips/Footstep1");
    }
	
	// Update is called once per frame
	virtual protected void Update () {


        move = false;
        
        float moveVertical = Input.GetAxis("Vertical");
        float lookHorizontal = Input.GetAxis("RightH");
        float lookVertical = Input.GetAxis("RightV");

        if(Input.GetKey(KeyCode.D))
            lookHorizontal = 1; 
        if(Input.GetKey(KeyCode.A))
            lookHorizontal = -1;

        if (moveVertical == 1.0)
        {
            move = true;
            transform.position = transform.position + cam.transform.forward * 5 * Time.deltaTime;
            if (audio_source.isPlaying == false)// add more logic later such as, onground/jumping etc etc
            {
               // AudioSource.PlayClipAtPoint(footstep_Sound1, transform.position);
               // footstep_playing = 1;
                audio_source.pitch = Random.Range(0.8f, 1);
                audio_source.volume = Random.Range(0.8f, 1.1f);
                audio_source.Play();
            }
        }

        animator.SetBool("Movement", move);



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

    protected void Throw()
    {
        carriedObject.GetComponent<Rigidbody>().isKinematic = false;
        carrying = false;
        carriedObject.GetComponent<Rigidbody>().AddForce(cam.transform.TransformDirection(Vector3.forward) * 100);
    }

    protected void PickUp()
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
		
	}
}
