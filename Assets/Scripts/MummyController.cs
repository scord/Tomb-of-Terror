using UnityEngine;
using System.Collections;
using System;

public class MummyController : PlayerController {

	private float murmurTimer;

    private AudioSource shout;

    bool showText = false;
    RaycastHit Mummy_ray = new RaycastHit();
    protected override void Start(){
		base.Start();

        
        shout = gameObject.GetComponent<AudioSource>();
        soundVision = cam.gameObject.GetComponent<SoundVision>();
        murmurTimer = 0.0f;

        StartConfig(m_GameParams.mainLevel);

	}

	protected override void Update(){

        murmurTimer += Time.deltaTime;

		base.Update();
	    if (Input.GetButtonDown("Fire1")){
            shout.volume = 2.0f;
            shout.Play();
            soundVision.EchoLocate();

            murmurTimer = 0.0f;

        }

        if (murmurTimer > 3.0f){

            shout.volume = 1.0f;
            shout.Play();

            murmurTimer = 0.0f;
        }

        if ( pickupEnabled && Input.GetButtonDown("Fire2")) {
            PickUp();
        }


        bool found_mummy = false;
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out Mummy_ray, 24))
        {
            if (Mummy_ray.collider.gameObject.tag == "Explorer")
            {
                OVRPlayerController mummy_controller = gameObject.GetComponent<OVRPlayerController>();
                mummy_controller.SetMoveScaleMultiplier(2.0f);
                found_mummy = true; 
            } 
        }

        if (!found_mummy)
        {
            OVRPlayerController mummy_controller = gameObject.GetComponent<OVRPlayerController>();
            mummy_controller.SetMoveScaleMultiplier(1.0f);
        }

    }

    /*void OnTriggerEnter(Collider col)
    {
        Debug.Log("WAS HERE");
        if (col.gameObject.tag == "Explorer")
        {
            //mummy wins
            showText = true;
            //wait a few seconds
            //application.loadscene(menu); or something
        }
    }*/

    protected override void PickUp() {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 16))
        {
            if (hit.collider.gameObject.tag == "Explorer")
            {
                CallEventPickUp(hit.collider.gameObject);
            }
        }
    }




    void OnGUI()
    {

        if (showText)
        {
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Mummy winsXXX!");
        }

    }

    protected override void ChangeLevel() {
        GameObject.Find("NetworkManager").GetComponent<NetworkManagerCustom>().ChangeLevel();
    }

    public override string GetPrizeTag() {
        return "Explorer";
    }

    public override void StartConfig(bool isMainLevel) {
        base.StartConfig(isMainLevel);
        if (isMainLevel) {
            soundVision.enabled = true;
            cam.backgroundColor = new Color (0, 0, 0, 1);
            cam.cullingMask = (cam.cullingMask ) &  ~(1 << LayerMask.NameToLayer("Ignore Sound Vision"));
            cam.clearFlags = CameraClearFlags.SolidColor;
        }
    }
}
