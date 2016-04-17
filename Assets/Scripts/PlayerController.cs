using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public Camera cam;
	public SoundVision soundVision;
    public AudioSource audio_source;
    GameObject carriedObject;
    public bool carrying;
    protected bool turned;
    protected bool move;
    public GameObject model;
    public GameObject player_tag;
    public Renderer renderer;
    public Shader standardShader;
    public Shader glowShader;
    bool showText= false;
	// public SoundVision test;
    // Use this for initialization

    public Animator animator;


    public delegate void PickUpDelegate(GameObject go);
    public event PickUpDelegate EventPickUp;

    public delegate void ThrowDelegate(GameObject go, Vector3 direction);
    public event ThrowDelegate EventThrow;

    virtual protected void Start () {
        GetComponent<IKHandler>().enabled = true;
        GetComponent<OVRPlayerController>().enabled = true;
        GetComponent<OVRSceneSampleController>().enabled = true;
	    turned = false;
        carrying = false;
        carriedObject = null;
        animator = GetComponent<Animator>();
        audio_source.clip = (AudioClip)Resources.Load("AudioClips/Footstep1");

        if (player_tag == null)
        {
            player_tag = GameObject.FindGameObjectWithTag("Player");
        }


        standardShader = Shader.Find("Standard");
        glowShader = Shader.Find("Custom/ItemGlow");

    }

	// Update is called once per frame
	virtual protected void Update () {
        move = false;

        float moveVertical = Input.GetAxis("Vertical");
        //float lookHorizontal = Input.GetAxis("RightH");
        //float lookVertical = Input.GetAxis("RightV");

        if (moveVertical == 1.0)
        {
            move = true;

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

        if (carrying)
        {
            carriedObject.transform.position = cam.transform.position + cam.transform.TransformDirection(Vector3.forward)*2;
        }

    }

    protected void Throw()
    {
        //carriedObject.GetComponent<Rigidbody>().isKinematic = false;
        carrying = false;

        //carriedObject.GetComponent<Rigidbody>().AddForce(cam.transform.TransformDirection(Vector3.forward) * 100);
        //carriedObject.GetComponent<Rigidbody>().AddTorque(new Vector3(1, 1, 1));
        //carriedObject.GetComponent<Object_SyncPosition>().Throw();
        if (EventThrow != null) {
            EventThrow(carriedObject, cam.transform.TransformDirection(Vector3.forward) * 600);
        }
        carriedObject = null;

        // carriedObject.GetComponent<Rigidbody>().AddForce(cam.transform.TransformDirection(Vector3.forward) * 200);
        // carriedObject.GetComponent<Rigidbody>().AddTorque(new Vector3(1, 1, 1));
        // if (carriedObject.GetComponent<Renderer>() != null)
        // {
        //     if (renderer.material.shader == standardShader)
        //     {
        //         renderer.material.shader = glowShader;
        //     }
        // }

    }

    protected void PickUp()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 16))
        {
            if (hit.collider.gameObject.tag == "PickUp")
            {
                carriedObject = hit.collider.gameObject;
                carriedObject.GetComponent<Rigidbody>().isKinematic = true;
                if (carriedObject.GetComponent<Renderer>() != null)
                {
                    renderer = carriedObject.GetComponent<Renderer>();
                    if (renderer.material.shader == glowShader)
                    {
                        renderer.material.shader = standardShader;
                    }
                }
                carrying = true;
                if ( EventPickUp != null ) {
                    EventPickUp(carriedObject);
                }
                //carriedObject.GetComponent<Object_SyncPosition>().PickUp("something");
            }
        }
    }

    public GameObject GetCarriedObject() {
        return carriedObject;
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("WAS HERE");
        if (col.gameObject.tag == "Mummy")
        {
            //mummy wins
            showText = true;
            //wait a few seconds
            //application.loadscene(menu); or something
        }
    }

    void OnGUI()
    {

        if (showText)
        {
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Mummy winsXXX!");
        }

    }
}
