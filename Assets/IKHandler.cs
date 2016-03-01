using UnityEngine;
using System.Collections;

public class IKHandler : MonoBehaviour {

    Animator animator;


    public float ikWeight;
    public float lookIKweight;
    public float bodyWeight;
    public float headWeight;
    public float eyesWeight;
    public float clampWeight;
    public Vector3 lookPos;
    public GameObject lookObject;
    public Camera cam;


	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
 
	}
	
	// Update is called once per frame
	void Update () {
      
	
	}

    void OnAnimatorIK()
    {
        animator.SetLookAtWeight(lookIKweight, bodyWeight, headWeight, eyesWeight, clampWeight);
        lookPos = cam.transform.position + Quaternion.Euler(cam.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - lookObject.transform.localRotation.eulerAngles.y, cam.transform.rotation.eulerAngles.z)*Vector3.forward*5;

        animator.SetLookAtPosition(lookPos);
    }
}
