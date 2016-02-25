using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private InteractScript trig;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 moveCamera = Vector3.zero;
    private Light torchIntensity;
    private bool onTrigger = false;
    private float wheelDirection;

    public Camera cam;
	public SoundVision soundVision;
    public AudioSource audio_source;
    public Animator animator;
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;

    bool turned;
    // public SoundVision test;
    // Use this for initialization
    void Start () {
        // turned = false;

        // soundVision = GetComponent<SoundVision>();
        // audio_source = GetComponent<AudioSource>();
        // audio_source.clip = (AudioClip)Resources.Load("AudioClips/Footstep1");
        animator = GetComponent<Animator>();
        torchIntensity = GetComponentsInChildren<Light>()[0];
        controller = GetComponent<CharacterController>();
    }
    


    // Update is called once per frame
    void Update() {
        bool move = false;
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (moveHorizontal == 1.0 || moveVertical == 1.0)
            move = true;

        animator.SetBool("Movement", move);

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
        
        wheelDirection = Input.GetAxis("Mouse ScrollWheel");
        if (wheelDirection > 0)
            torchIntensity.intensity += 0.20f;
        else if (wheelDirection <  0)
                torchIntensity.intensity -= 0.20f;


        // deal with in-game interactions
        if(onTrigger && trig.withKey){
            if(Input.GetKeyDown(KeyCode.E)){
                trig.Interact();
            }
        }

    }


    void OnTriggerEnter(Collider other){

        if(other.tag.Equals("Interaction")){
            trig = (InteractScript) other.GetComponent(typeof(InteractScript));
            onTrigger = true;
            if(trig.withKey)
                trig.PreInteract();
            else trig.Interact();
        }
    }

    void OnTriggerStay(Collider other){
        if(other.tag.Equals("Interaction") && !onTrigger){
            onTrigger = true;
            trig.PreInteract(); 
        }
    }


    void OnTriggerExit(Collider other){
        if(other.tag.Equals("Interaction")){
            onTrigger =false;
            trig.EndInteract();
        }
    }
}
