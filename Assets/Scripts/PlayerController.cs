using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public Camera cam;
	public SoundVision soundVision;
    public AudioSource audio_source;
    protected GameObject carriedObject;
    public bool carrying;
    protected bool turned;
    public GameObject model;
    public GameObject player_tag;
    public Renderer m_Renderer;
    public Shader standardShader;
    public Shader glowShader;
    private AudioClip pick_up_gold;

    private bool m_PickupEnabled = true;

    public bool pickupEnabled { get { return m_PickupEnabled; } set { m_PickupEnabled = value; }}
    //bool showText= false;
	// public SoundVision test;
    // Use this for initialization

    public Animator animator;

    [SerializeField] private IntroTutorialScript m_IntroTutorialScript;
    [SerializeField] protected VibrationController m_VibrationController;
    public delegate void PickUpDelegate(GameObject go);
    public event PickUpDelegate EventPickUp;

    public delegate void ThrowDelegate(GameObject go, Vector3 direction);
    public event ThrowDelegate EventThrow;

    protected bool m_IsMainLevel = true;

    [SerializeField] protected GameParams m_GameParams;
    void Awake() {
        GameObject go = GameObject.Find("GameParams");
        m_GameParams = go.GetComponent<GameParams>();
    }

    virtual protected void Start () {
        GetComponent<IKHandler>().enabled = true;
        GetComponent<OVRPlayerController>().enabled = true;
        GetComponent<OVRSceneSampleController>().enabled = true;
	    turned = false;
        carrying = false;
        carriedObject = null;
        animator = GetComponent<Animator>();
        pick_up_gold = (AudioClip)Resources.Load("AudioClips/pickup_gold_00");

        if (player_tag == null)
        {
            player_tag = GameObject.FindGameObjectWithTag("Player");
        }


        standardShader = Shader.Find("Standard");
        glowShader = Shader.Find("Custom/ItemGlow");

    }

	// Update is called once per frame
	virtual protected void Update () {
        if (!m_IsMainLevel && Input.GetKeyDown(KeyCode.K)) {
            ChangeLevel();
        }

        if (carrying)
        {
            carriedObject.transform.position = cam.transform.position + cam.transform.TransformDirection(Vector3.forward)*2;
        }

    }

    protected virtual void Throw(){

    }

    protected bool isServerChecking = false;
    protected virtual void PickUp()
    {
    }

    public virtual void CallbackServerChecking(bool success, string tag) {
        isServerChecking = false;
        Debug.Log("Result was a success? " + success);
    }

    protected void CallEventPickUp(GameObject go) {
        Debug.Log("I call");
        if ( EventPickUp != null ) {
            EventPickUp(go);
            if (go.tag == "Prize" || go.tag == "SmallPrize")
            {
                AudioSource.PlayClipAtPoint(pick_up_gold, transform.position);
            }
        }
    }

    protected void CallEventThrow(GameObject go, Vector3 v) {
        if (EventThrow != null) {
            EventThrow(go, v);
        }
    }

    public virtual string[] GetPrizeTags() {
        return new string[] {};
    }

    public GameObject GetCarriedObject() {
        return carriedObject;
    }

    public virtual void StartConfig(bool isMainLevel) {
        m_IntroTutorialScript.enabled = !isMainLevel;
        if (isMainLevel) {
            m_IsMainLevel = true;
        } else {
            m_IsMainLevel = false;
            //GetComponent<Explorer_HeartRate>().enabled = false;
        }
    }

    protected virtual void OnDisable() {
        m_IntroTutorialScript.enabled = false;
        m_IntroTutorialScript.gameObject.SetActive(false);
    }

    protected virtual void ChangeLevel() {}

    /*void OnTriggerEnter(Collider col)
    {
        Debug.Log("WAS HERE");
        if (col.gameObject.tag == "Mummy")
        {
            //mummy wins
            showText = true;
            //wait a few seconds
            //application.loadscene(menu); or something
        }
    }

    void OnGUI()
    {

        if (showText)
        {
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Mummy winsXXX!");
        }

    }*/
}



