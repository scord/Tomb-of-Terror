﻿using UnityEngine;
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
    public Renderer renderer;
    public Shader standardShader;
    public Shader glowShader;
	// public SoundVision test;
    // Use this for initialization

    public Animator animator;

    virtual protected void Start () {
        GetComponent<OVRPlayerController>().enabled = true;
        GetComponent<OVRSceneSampleController>().enabled = true;
	    turned = false;
        carrying = false;
        animator = GetComponent<Animator>();
        audio_source.clip = (AudioClip)Resources.Load("AudioClips/Footstep1");

        standardShader = Shader.Find("Standard");
        glowShader = Shader.Find("Custom/ItemGlow");
    }
	
	// Update is called once per frame
	virtual protected void Update () {
        move = false;
        
        float moveVertical = Input.GetAxis("Vertical");
        float lookHorizontal = Input.GetAxis("RightH");
        float lookVertical = Input.GetAxis("RightV");

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
        carriedObject.GetComponent<Rigidbody>().isKinematic = false;
        carrying = false;
        carriedObject.GetComponent<Rigidbody>().AddForce(cam.transform.TransformDirection(Vector3.forward) * 200);
        carriedObject.GetComponent<Rigidbody>().AddTorque(new Vector3(1, 1, 1));
        if (carriedObject.GetComponent<Renderer>() != null) 
        {
            if (renderer.material.shader == standardShader) 
            {
                renderer.material.shader = glowShader;
            }
        }
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
            }
        }
    }

	void OnTriggerEnter(Collider other) {
		
	}
}
