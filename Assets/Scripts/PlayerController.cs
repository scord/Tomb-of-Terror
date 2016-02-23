﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public Camera cam;
	public SoundVision soundVision;
    public AudioSource audio_source;
    public Animator animator;

    bool turned;
	// public SoundVision test;
    // Use this for initialization
    void Start () {
	    // turned = false;

      // soundVision = GetComponent<SoundVision>();
        // audio_source = GetComponent<AudioSource>();
        // audio_source.clip = (AudioClip)Resources.Load("AudioClips/Footstep1");
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private float wheelDirection;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 moveCamera = Vector3.zero;
    private Light torchIntensity;
    void Update() {
        bool move = false;
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (moveHorizontal == 1.0 || moveVertical == 1.0)
            move = true;

        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded) {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveCamera = new Vector3(0, Input.GetAxis("Mouse X"), 0 );

            // transform.rotation = transform.rotation + moveCamera;
            transform.Rotate(moveCamera);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
            
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        torchIntensity = GetComponentsInChildren<Light>()[0];
        wheelDirection = Input.GetAxis("Mouse ScrollWheel");
        if (wheelDirection > 0)
            torchIntensity.intensity += 0.20f;
        else if (wheelDirection <  0)
                torchIntensity.intensity -= 0.20f;

        animator.SetBool("Movement", move);
    }

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Explorer") {
			Debug.Log("MUMMY WINS");
		}
	}
}
