using UnityEngine;
using System.Collections;
using UnityEngine.UI;
 using System.Collections.Generic;

public class MummyIntroScript : IntroTutorialScript {

	private SoundVision vision;
	private GameObject mummy;
	private Camera view;
	[SerializeField] private MummyController mummyController;
	[SerializeField] private GameObject mummyObject;

	// look around variable
	[SerializeField] private GameObject headCanvas;
	private float lookAround = 3;

	// walk variable
	[SerializeField] private GameObject walkCanvas;
	private float walkTime = 2;

	// run variables
	[SerializeField] private GameObject runCanvas;
	private float runTime = 2;   // seconds
	private bool running = false;

	// echolocaltion variables
	[SerializeField] private GameObject echolocateCanvas;

	// catch explorer variables
	[SerializeField] private GameObject catchCanvas;

	// pivot variables
	[SerializeField] private GameObject pivotCanvas;
	private int pivotCount = 6;

	// prepare variables
	[SerializeField] private GameObject prepareCanvas;
	private float prepareTime = 3;
	private int completedPrompts = 0;
	private List<string> preparePrompts = new List<string>{"The following are just preparation for when you become blind.",
																													"Your goal is to catch the explorer",
																													"You will need to make sounds to be able to navigate",
																													"You will also be able to 'see' fires, oppening doors and the explorer's heart and footsteps ",
																													"Remember, if you don't see anything, trigger echolocation",
																													"Do it now, to enter the soundvision and try and find the explorer"};

  // Use this for initialization
  protected void Start () {
    walkCanvas.SetActive(false);
    pivotCanvas.SetActive(false);
    echolocateCanvas.SetActive(false);
		runCanvas.SetActive(false);
		catchCanvas.SetActive(false);
		prepareCanvas.SetActive(false);
		headCanvas.SetActive(true);

    walkCanvas.GetComponent<CanvasGroup>().alpha = 0;
    pivotCanvas.GetComponent<CanvasGroup>().alpha = 0;
    echolocateCanvas.GetComponent<CanvasGroup>().alpha = 0;
		runCanvas.GetComponent<CanvasGroup>().alpha = 0;
		catchCanvas.GetComponent<CanvasGroup>().alpha = 0;
		prepareCanvas.GetComponent<CanvasGroup>().alpha = 0;

		prepareCanvas.GetComponent<Text>().text = preparePrompts[completedPrompts];
		prepareTime = preparePrompts[completedPrompts].Length / 8;

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
      if( walkTime > 0)
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
				FadeTo(runCanvas, prepareCanvas);
		}

		else if(prepareCanvas.activeSelf){
			if(completedPrompts < preparePrompts.Count){
				if( prepareTime > 0)
					prepareTime -= Time.deltaTime;
				else {
					completedPrompts++;
					prepareCanvas.GetComponent<Text>().text = preparePrompts[completedPrompts];
					prepareTime = preparePrompts[completedPrompts].Length / 8;
				}
			}
				else
					FadeTo(prepareCanvas, catchCanvas);
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
		prepareCanvas.SetActive(false);
  }
}
