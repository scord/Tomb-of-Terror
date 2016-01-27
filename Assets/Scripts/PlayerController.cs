using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {


    //[SerializeField] private Camera m_Camera;
    //[SerializeField] private SoundVision m_SoundsVision;
    [SerializeField] private bool is_Walking;
    [SerializeField] private float m_Speed = 2.0f;
    private Vector3 m_MoveDirection = Vector3.zero;
    private Vector2 m_Input;

    public Camera cam;
	public SoundVision soundVision;

    bool turned;
	// public SoundVision test;
    // Use this for initialization
    void Start () {
	    turned = false;
	}
	
	// Update is called once per frame
	void Update () {
        
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 moveDirection = transform.forward*moveVertical + transform.right*moveHorizontal;
        if (moveVertical == 1.0)
        {
            transform.position = transform.position + cam.transform.forward * m_Speed * Time.deltaTime;
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
