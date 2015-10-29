using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public Camera cam;
    bool turned;
    // Use this for initialization
    void Start () {
	    turned = false;
	}
	
	// Update is called once per frame
	void Update () {
        
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (moveVertical == 1.0)
        {
            transform.position = transform.position + cam.transform.forward * 2 * Time.deltaTime;
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
}
