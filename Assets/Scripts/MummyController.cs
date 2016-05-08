using UnityEngine;
using System.Collections;
using System;

public class MummyController : PlayerController {

	private float murmurTimer;

    private AudioSource shout;
    [SerializeField] private AudioClip swipe_sound;

    bool showText = false;
		private bool finishedTutorial = false;
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
    if (Input.GetButtonDown("Fire1") && murmurTimer > 0.5f){
			if(!shout.isPlaying){
				// shout.volume = 1.0f;
				shout.Play();
			}
			soundVision.EchoLocate(murmurTimer);
//			Debug.Log(murmurTimer);
      murmurTimer = 0.0f;
    }

    if (murmurTimer%4 > 3.0f && !shout.isPlaying){

        // shout.volume = 1.0f;

     //   shout.Play();

     //   murmurTimer+=1.0f;
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
            if (!isServerChecking && hit.collider.gameObject.tag == "Explorer")
            {
                CallEventPickUp(hit.collider.gameObject);
                isServerChecking = true;
            }
        }
    }

    public override void CallbackServerChecking(bool success, string tag) {
        if (success) {
            m_VibrationController.VibrateFor(1.0f);
            AudioSource.PlayClipAtPoint(swipe_sound, transform.position);
            StartCoroutine(DelayedResponseServer(success, tag));
        } else {
            base.CallbackServerChecking(success, tag);
        }
    }

    private IEnumerator DelayedResponseServer(bool success, string tag) {
        yield return new WaitForSeconds(1.0f);
        base.CallbackServerChecking(success, tag);
    }

		public void FinishTutorial(){
			finishedTutorial = true;
		}

		public bool CheckTutorial(){
			return finishedTutorial;
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

    public override string[] GetPrizeTags() {
        return new string[] {"Explorer"};
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
