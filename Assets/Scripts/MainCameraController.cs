using UnityEngine;
using System.Collections;

public class MainCameraController : MonoBehaviour {

  [SerializeField] private Camera m_Camera;

  private int speed = 20;
  private Transform camTransform;
	// Use this for initialization
	void Start () {
    camTransform = m_Camera.transform;
	}
	
	// Update is called once per frame
	void Update () {
    Cursor.visible = true;
    speed  = ( Input.GetKey(KeyCode.LeftShift)) ? 40 : 20;
    if ( Input.GetKey(KeyCode.W)) camTransform.position += camTransform.forward*speed*Time.deltaTime;
    if ( Input.GetKey(KeyCode.S)) camTransform.position -= camTransform.forward*speed*Time.deltaTime; 
    if ( Input.GetKey(KeyCode.D)) camTransform.position += camTransform.right*speed*Time.deltaTime; 
    if ( Input.GetKey(KeyCode.A)) camTransform.position -= camTransform.right*speed*Time.deltaTime;  
    if ( Input.GetKey(KeyCode.Q)) camTransform.position += camTransform.up*speed*Time.deltaTime;  
    if ( Input.GetKey(KeyCode.E)) camTransform.position -= camTransform.up*speed*Time.deltaTime;    

	}
}
