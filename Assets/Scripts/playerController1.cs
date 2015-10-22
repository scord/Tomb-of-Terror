using UnityEngine;
using System.Collections;

public class playerController1 : MonoBehaviour {
	private Rigidbody rb;
	public float speed;
	public float rotateSpeed;
	private float curSpeed;
	public GameObject flashlight;
	public float flashSpeed;
	
	void Start(){
		rb =GetComponent<Rigidbody>();
	}
	
	void FixedUpdate(){
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
	 	
		
		Vector3 movement = new Vector3 (0.0f, 0.0f, moveVertical);
		Vector3 rotate = new Vector3 ( 0.0f, moveHorizontal, 0.0f);
		transform.Rotate (rotate * rotateSpeed);

		Vector3 forward = transform.TransformDirection(Vector3.forward);
		curSpeed = speed * moveVertical;
		if (moveVertical == 0.0f) {
			flashlight.GetComponent<Light>().intensity -= flashSpeed;
		} else {
			flashlight.GetComponent<Light>().intensity += flashSpeed;
		}

		rb.MovePosition (transform.position+ forward*curSpeed);
	}

	
	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag("pickup"))
			other.gameObject.SetActive(false);
		
	}	
}
