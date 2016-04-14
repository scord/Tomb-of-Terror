using UnityEngine;
using System.Collections;

public class IKHandler : MonoBehaviour {

    Animator animator;


    public float ikWeight;
    public float lookIKweight;
    public float bodyWeight;
    public float headWeight;
    public float footWeight;
    public float eyesWeight;
    public float clampWeight;
    public Vector3 lookPos;
    public GameObject lookObject;
    public Camera cam;

    public Transform leftFootTarget;
    public Transform rightFootTarget;

    public Transform leftFootHint;
    public Transform rightFootHint;

    Vector3 leftFootPos;
    Vector3 rightFootPos;

    Quaternion leftFootRot;
    Quaternion rightFootRot;

    public Transform rightFoot;
    public Transform leftFoot;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        cam = GetComponentInChildren<Camera>();

    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit leftHit;
        RaycastHit rightHit;

        Vector3 lpos = leftFoot.TransformPoint(Vector3.zero);
        Vector3 rpos = rightFoot.TransformPoint(Vector3.zero);

        if (Physics.Raycast(lpos, -Vector3.up, out leftHit, 1))
        {
            leftFootPos = leftHit.point;
            leftFootRot = Quaternion.FromToRotation(transform.up, leftHit.normal) * transform.rotation;
        }
        if (Physics.Raycast(rpos, -Vector3.up, out rightHit, 1))
        {
            rightFootPos = rightHit.point;
            rightFootRot = Quaternion.FromToRotation(transform.up, rightHit.normal) * transform.rotation;
        }



    }

    void OnAnimatorIK()
    {
        Debug.Log("TESTING");
        animator.SetLookAtWeight(lookIKweight, bodyWeight, headWeight, eyesWeight, clampWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, footWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, footWeight);

        animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPos);
        animator.SetIKPosition(AvatarIKGoal.LeftFoot, rightFootPos);

        animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, footWeight);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, footWeight);

        animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRot);
        animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRot);

       /* animator.SetIKHintPosition(AvatarIKHint.LeftKnee, leftFootTarget.position);
        animator.SetIKHintPosition(AvatarIKHint.RightKnee, rightFootTarget.position);
        */
        lookPos = cam.transform.position + Quaternion.Euler(cam.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - lookObject.transform.localRotation.eulerAngles.y, cam.transform.rotation.eulerAngles.z)*Vector3.forward*5;
        
        animator.SetLookAtPosition(lookPos);
    }
}
