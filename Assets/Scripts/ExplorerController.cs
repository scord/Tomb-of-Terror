using UnityEngine;
using System.Collections;
using System;

public class ExplorerController : PlayerController {

	//private Light torchIntensity;

    [SerializeField] private GameObject m_Torch;
    private TorchManager torchManagerScript;

	private float wheelDirection;
	private bool onTrigger;

    private float firstMarkSpace = 0.5F;
    private double heartVolumeExpScale = 1.035f;
    private HeartRateManager HRManager;

    private float heartBeatTimer;
    private bool HRAudioSelect;
    private int HR;
    public int normalHR = 65;


	protected override void Start(){

        HRAudioSelect = true;

		base.Start();
		//torchIntensity = GetComponentsInChildren<Light>()[0];

        heartBeatTimer = 0.0f;
        
        HRManager = GameObject.Find("HeartRate").GetComponent<HeartRateManager>();
        if (!canChangeLevel) {
            m_Torch.SetActive(true);
            torchManagerScript = new TorchManager(m_Torch);
            torchManagerScript.Trigger(true);
        }
        
	}



    public override void StartConfig(bool isMainLevel) {
        base.StartConfig(isMainLevel);
        if (isMainLevel) {
            canChangeLevel = false;
        } else {
            canChangeLevel = true;
            //GetComponent<Explorer_HeartRate>().enabled = false;
        }
    }
	protected override void Update(){
		base.Update();

        if (Input.GetButtonDown("Fire2"))
            if (!carrying)
                PickUp();
            else
                Throw();

        if (torchManagerScript != null) {
            torchManagerScript.SetLight();
            if (Input.GetButtonDown("Fire1")) {
                torchManagerScript.Trigger();
            }

        }   
        // deal with in-game interactions
        /*if (onTrigger && trig != null && trig.withKey){
            if(Input.GetKeyDown(m_TriggerKey)){
                trig.Interact();
            }
        }*/
	}

    public TorchManager GetTorchManager() {
        return torchManagerScript;
    }

    public bool GetTorchState() {
        return torchManagerScript.GetState();
    }

    void OnDisable() {
        m_Torch.SetActive(false);
        GetComponent<Explorer_HeartRate>().enabled = false;   
    }

    protected override void ChangeLevel() {
        GameObject.Find("NetManager").GetComponent<NetManager>().ChangeLevel(2);
    }

}
