using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExplorerController : PlayerController {

	//private Light torchIntensity;

    [SerializeField] private GameObject m_Torch;
    [SerializeField] private GameObject m_PointsCanvas;
    private TorchManager torchManagerScript;

	private float wheelDirection;
	private bool onTrigger;
    public bool carryingTorch;

	protected override void Start(){

		base.Start();
		//torchIntensity = GetComponentsInChildren<Light>()[0];
        InstantiateTorch();
        StartConfig(m_GameParams.mainLevel);
        carryingTorch = false;

	}



    public override void StartConfig(bool isMainLevel) {
        base.StartConfig(isMainLevel);
        if (isMainLevel) {
            m_IsMainLevel = true;
            if ( m_PointsCanvas != null ) m_PointsCanvas.SetActive(true);
        } else {
            m_IsMainLevel = false;
            torchManagerScript = null;
            m_Torch.SetActive(false);
        }
    }
	protected override void Update(){
		base.Update();

        if (pickupEnabled && Input.GetButtonDown("Fire2"))
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

    protected override void OnDisable() {
        base.OnDisable();
        m_Torch.SetActive(false);
        GetComponent<Explorer_HeartRate>().enabled = false;
    }

    protected override void ChangeLevel() {
        GameObject.Find("NetworkManager").GetComponent<NetworkManagerCustom>().ChangeLevel();
    }

    protected override void Throw()
    {
        //carriedObject.GetComponent<Rigidbody>().isKinematic = false;
        carrying = false;

        //carriedObject.GetComponent<Rigidbody>().AddForce(cam.transform.TransformDirection(Vector3.forward) * 100);
        //carriedObject.GetComponent<Rigidbody>().AddTorque(new Vector3(1, 1, 1));
        //carriedObject.GetComponent<Object_SyncPosition>().Throw();
        CallEventThrow(carriedObject, cam.transform.TransformDirection(Vector3.forward) * 600);

        // carriedObject.GetComponent<Rigidbody>().AddForce(cam.transform.TransformDirection(Vector3.forward) * 200);
        // carriedObject.GetComponent<Rigidbody>().AddTorque(new Vector3(1, 1, 1));
        if (carriedObject.GetComponent<Renderer>() != null)
        {
            if (m_Renderer.material.shader == standardShader)
            {
                m_Renderer.material.shader = glowShader;
            }
        }
        carriedObject = null;

    }

    protected override void PickUp()
    {
        RaycastHit hit = new RaycastHit();
        Debug.Log("I try to pick, " + isServerChecking);
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 16))
        {
            if (hit.collider.gameObject.tag == "PickUp")
            {
                carriedObject = hit.collider.gameObject;
                carriedObject.GetComponent<Rigidbody>().isKinematic = true;
                if (carriedObject.GetComponent<Renderer>() != null)
                {
                    m_Renderer = carriedObject.GetComponent<Renderer>();
                    if (m_Renderer.material.shader == glowShader)
                    {
                        m_Renderer.material.shader = standardShader;
                    }
                }
                carrying = true;
                CallEventPickUp(carriedObject);
                //carriedObject.GetComponent<Object_SyncPosition>().PickUp("something");
            } else if ( !isServerChecking && (System.Array.IndexOf(GetPrizeTags(), hit.collider.gameObject.tag) != -1)) {
                isServerChecking = true;
                CallEventPickUp(hit.collider.gameObject);
            }
        }
    }

    public override void CallbackServerChecking(bool success, string tag) {
        base.CallbackServerChecking(success, tag);
        if (success) {
            if (!m_IsMainLevel && tag == "Prize") {
                InstantiateTorch();
            }
        }
    }

    public override string[] GetPrizeTags() {
        return new string[] {"Prize", "SmallPrize"};
    }

    private void InstantiateTorch() {
        if (torchManagerScript == null) {
            m_Torch.SetActive(true);
            torchManagerScript = new TorchManager(m_Torch);
            torchManagerScript.Trigger(true);
        }
        carryingTorch = true;
    }

}
