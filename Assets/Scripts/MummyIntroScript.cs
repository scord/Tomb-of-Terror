using UnityEngine;
using System.Collections;

public class MummyIntroScript : IntroTutorialScript {

	[SerializeField] private GameObject headCanvas, walkCanvas, pivotCanvas, echolocateCanvas, runCanvas, catchCanvas;
	[SerializeField] private MummyController mummyController;
	[SerializeField] private GameObject mummyObject;
	private int pivotCount = 5;
	// [SerializeField] private GameObject trapTrigger, coffinEnter, coffinOut;

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

    FadeToWalk();

		mummyController = mummyObject.GetComponent<MummyController>();
  }



  protected void Awake () {
    /*headCanvas = GameObject.Find("Controller-Head");
    walkCanvas = GameObject.Find("Controller-Walk");
    pivotCanvas = GameObject.Find("Controller-Pivot");
    echolocateCanvas = GameObject.Find("Controller-Echolocate");*/
		// trapTrigger = GameObject.Find("TrapTrigger");
		// coffinEnter = GameObject.Find("TrapDoor");
		// coffinOut 	= GameObject.Find("CoffinDoor");
  }

  // Update is called once per frame
  protected void Update () {
		if ( (Input.GetAxis("Vertical") != 0) && walkCanvas.activeSelf) {
			FadeTo(walkCanvas, pivotCanvas);
		}
		// if pivot canvas activated
		else if (pivotCanvas.activeSelf) {
			if( EndPivot() )
				FadeTo(pivotCanvas, runCanvas);
		}
		else if( runCanvas.activeSelf ) {
      if( Input.GetKey(KeyCode.LeftShift) || OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0 ) {
        FadeTo(runCanvas, catchCanvas);
      }
    }
		else if (catchCanvas.activeSelf){
			if(Input.GetButtonDown("Fire2")){
				FadeTo(catchCanvas, echolocateCanvas);
			}
		}
		else if(echolocateCanvas.activeSelf){
			if((Input.GetButtonDown("Fire1"))){
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
