using UnityEngine;
using System.Collections;

public class MummyIntroScript : IntroTutorialScript {

	private SoundVision vision;
	private GameObject mummy;
	private Camera view;
	[SerializeField] private GameObject headCanvas, walkCanvas, pivotCanvas, echolocateCanvas, runCanvas, catchCanvas;
	[SerializeField] private MummyController mummyController;
	[SerializeField] private GameObject mummyObject;

	// look around variable
	private float lookAround = 3;

	// walk variable
	private float walkTime = 2;

	// torch variables
	private int torchPress = 2;

	// run variables
	private float runTime = 3;   // seconds
	private bool running = false;

	// pivot variables
	private int pivotCount = 6;


  // Use this for initialization
  protected void Start () {
    walkCanvas.SetActive(false);
    pivotCanvas.SetActive(false);
    echolocateCanvas.SetActive(false);
		runCanvas.SetActive(false);
		catchCanvas.SetActive(false);
		headCanvas.SetActive(true);

    walkCanvas.GetComponent<CanvasGroup>().alpha = 0;
    pivotCanvas.GetComponent<CanvasGroup>().alpha = 0;
    echolocateCanvas.GetComponent<CanvasGroup>().alpha = 0;
		runCanvas.GetComponent<CanvasGroup>().alpha = 0;
		catchCanvas.GetComponent<CanvasGroup>().alpha = 0;

		mummyController = mummyObject.GetComponent<MummyController>();
		mummy = mummyObject.transform.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").gameObject;

		vision = mummy.GetComponent<SoundVision>();
		view = mummy.GetComponent<Camera>();
  }

  // Update is called once per frame
  protected void Update () {
		// rotate head prompt
		if( headCanvas.activeSelf ){
			if(lookAround > 0)
				lookAround -= Time.deltaTime;
			else
				FadeToWalk();
		}

		// walk around prompt
    else if (walkCanvas.activeSelf) {
      if(Input.GetAxis("Vertical") != 0 && walkTime > 0)
        walkTime -= Time.deltaTime;
      else
        FadeTo(walkCanvas, pivotCanvas);
    }

		// if pivot canvas activated
		else if (pivotCanvas.activeSelf) {
			if( EndPivot() )
				FadeTo(pivotCanvas, runCanvas);
		}

		// if run canvas activated
		else if( runCanvas.activeSelf ) {
			if( Input.GetKey(KeyCode.LeftShift) || OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0 )
				running = true;
			else
				running = false;

			if(running && runTime > 0)
				runTime -= Time.deltaTime;

			if(runTime < 0)
				FadeTo(runCanvas, catchCanvas);
		}

		else if (catchCanvas.activeSelf){
			if(Input.GetButtonDown("Fire2")){
				FadeTo(catchCanvas, echolocateCanvas);
			}
		}

		else if(echolocateCanvas.activeSelf){
			if((Input.GetButtonDown("Fire1"))){
				TurnOnVision();
				echolocateCanvas.SetActive(false);
				mummyController.FinishTutorial();
			}
		}
  }


  private bool EndPivot(){
    Debug.Log(pivotCount);
    if(pivotCount > 0){
      if ( Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q) || OVRInput.Get(OVRInput.Button.PrimaryShoulder) || OVRInput.Get(OVRInput.Button.SecondaryShoulder)  ){
        Debug.Log("pressed");
        pivotCount--;
      }
      return false;
    }
    return true;
  }

	private void TurnOnVision(){
		vision.enabled = true;
		view.backgroundColor = new Color (0, 0, 0, 1);
		view.cullingMask = (view.cullingMask ) &  ~(1 << LayerMask.NameToLayer("Ignore Sound Vision"));
		view.clearFlags = CameraClearFlags.SolidColor;
	}

  private void FadeToWalk () {
    StartCoroutine(WaitFunction(4.0F));
  }

	private void FadeTo( GameObject fromCanvas, GameObject toCanvas){
    StartCoroutine(FadeOut(fromCanvas, 0.5F));
    StartCoroutine(FadeIn(toCanvas, 0.2F));
  }


  private void FadeToPivot () {
    StartCoroutine(FadeOut(walkCanvas, 0.3F));
    StartCoroutine(FadeIn(pivotCanvas, 0.1F));
  }

  IEnumerator WaitFunction (float waitTime)
  {
    yield return new WaitForSeconds(waitTime);
    StartCoroutine(FadeOut(headCanvas, 0.5F));
    StartCoroutine(FadeIn(walkCanvas, 0.2F));
  }

  IEnumerator FadeIn (GameObject obj, float speed) {
    float increment;
    obj.SetActive(true);
    CanvasGroup cv = obj.GetComponent<CanvasGroup>();
    while (cv.alpha < 1) {
        increment = speed * Time.deltaTime;
        if (cv.alpha + increment > 1) cv.alpha = 1;
        else cv.alpha += speed * Time.deltaTime;
        yield return null;
    }
  }

  IEnumerator FadeOut (GameObject obj, float speed) {
    Debug.Log("fade " + obj);
    float increment;
    CanvasGroup cv = obj.GetComponent<CanvasGroup>();
    while (cv.alpha > 0) {
        increment = speed * Time.deltaTime;
        if (cv.alpha - increment < 0) cv.alpha = 0;
        else cv.alpha -= speed * Time.deltaTime;
        yield return null;
    }
    obj.SetActive(false);
  }

  protected void OnDisable() {
    headCanvas.SetActive(false);
    walkCanvas.SetActive(false);
    pivotCanvas.SetActive(false);
    echolocateCanvas.SetActive(false);
		runCanvas.SetActive(false);
		catchCanvas.SetActive(false);
  }
}
