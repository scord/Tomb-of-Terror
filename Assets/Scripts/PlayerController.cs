using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public Camera cam;
	public SoundVision soundVision;
    public AudioSource audio_source;
    protected GameObject carriedObject;
    public bool carrying;
    protected bool turned;
    protected bool move;
    public GameObject model;
    public GameObject player_tag;
    public Renderer m_Renderer;
    public Shader standardShader;
    public Shader glowShader;

    private bool m_PickupEnabled = true;

    public bool pickupEnabled { get { return m_PickupEnabled; } set { m_PickupEnabled = value; }}
    //bool showText= false;
	// public SoundVision test;
    // Use this for initialization

    public Animator animator;

    [SerializeField] private IntroTutorialScript m_IntroTutorialScript;

    public delegate void PickUpDelegate(GameObject go);
    public event PickUpDelegate EventPickUp;

    public delegate void ThrowDelegate(GameObject go, Vector3 direction);
    public event ThrowDelegate EventThrow;

    protected bool canChangeLevel = true;

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
        audio_source.clip = (AudioClip)Resources.Load("AudioClips/Footstep1");

        if (player_tag == null)
        {
            player_tag = GameObject.FindGameObjectWithTag("Player");
        }


        standardShader = Shader.Find("Standard");
        glowShader = Shader.Find("Custom/ItemGlow");

    }

	// Update is called once per frame
	virtual protected void Update () {
        move = false;
        if (canChangeLevel && Input.GetKeyDown(KeyCode.K)) {
            ChangeLevel();
        }

        float moveVertical = Input.GetAxis("Vertical");
        //float lookHorizontal = Input.GetAxis("RightH");
        //float lookVertical = Input.GetAxis("RightV");

        if (moveVertical == 1.0)
        {
            move = true;

            if (audio_source.isPlaying == false)// add more logic later such as, onground/jumping etc etc
            {
               // AudioSource.PlayClipAtPoint(footstep_Sound1, transform.position);
               // footstep_playing = 1;
                audio_source.pitch = Random.Range(0.8f, 1);
                audio_source.volume = Random.Range(0.8f, 1.1f);
                audio_source.Play();
            }
        }

        animator.SetBool("Movement", move);

        if (carrying)
        {
            carriedObject.transform.position = cam.transform.position + cam.transform.TransformDirection(Vector3.forward)*2;
        }

    }

    protected virtual void Throw()
    {

    }

    protected virtual void PickUp()
    {
    }

    protected void CallEventPickUp(GameObject go) {
        Debug.Log("I call");
        if ( EventPickUp != null ) {
            EventPickUp(go);
        }
    }

    protected void CallEventThrow(GameObject go, Vector3 v) {
        if (EventThrow != null) {
            EventThrow(go, v);
        }
    }

    public virtual string GetPrizeTag() {
        return "";
    }

    public GameObject GetCarriedObject() {
        return carriedObject;
    }

    public virtual void StartConfig(bool isMainLevel) {
        m_IntroTutorialScript.enabled = !isMainLevel;
        if (isMainLevel) {
            canChangeLevel = false;
        } else {
            canChangeLevel = true;
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
