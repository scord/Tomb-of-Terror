using UnityEngine;
using System.Collections;

public class MummyIntroScript : IntroTutorialScript {

	private GameObject headCanvas, walkCanvas, pivotCanvas, echolocateCanvas;
	private GameObject trapTrigger, coffinEnter, coffinOut;

  // Use this for initialization
  protected void Start () {
    headCanvas.SetActive(true);
    walkCanvas.SetActive(false);
    pivotCanvas.SetActive(false);
    echolocateCanvas.SetActive(false);

    walkCanvas.GetComponent<CanvasGroup>().alpha = 0;
    pivotCanvas.GetComponent<CanvasGroup>().alpha = 0;
    echolocateCanvas.GetComponent<CanvasGroup>().alpha = 0;

    FadeToWalk();
  }



  protected void Awake () {
    headCanvas = GameObject.Find("Controller-Head");
    walkCanvas = GameObject.Find("Controller-Walk");
    pivotCanvas = GameObject.Find("Controller-Pivot");
    echolocateCanvas = GameObject.Find("Controller-Echolocate");
		trapTrigger = GameObject.Find("TrapTrigger");
		coffinEnter = GameObject.Find("TrapDoor");
		coffinOut 	= GameObject.Find("CoffinDoor");
  }

  // Update is called once per frame
  protected void Update () {
    if ( (Input.GetAxis("Vertical") != 0) && walkCanvas.activeSelf) {
      FadeToPivot();
    }
    else if ( (Input.GetAxis("Horizontal") != 0) && pivotCanvas.activeSelf) {
      StartCoroutine(FadeOut(pivotCanvas, 0.05F));
    }
  }

  private void FadeToWalk () {
    StartCoroutine(WaitFunction(4.0F));
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
  }
}
