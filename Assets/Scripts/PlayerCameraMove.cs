using UnityEngine;
using System.Collections;

public class PlayerCameraMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	private Vector3 moveCamera;
	// Update is called once per frame
	void Update () {
			moveCamera = new Vector3(-Input.GetAxis("Mouse Y"), 0, 0 );

            transform.Rotate(moveCamera);
	}
}
