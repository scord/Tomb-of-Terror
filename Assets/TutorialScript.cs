using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialScript : PlayerController {

  private GameObject headCanvas, walkCanvas, pivotCanvas, pickupCanvas, throwCanvas;

  // Use this for initialization
  void Start () {
    headCanvas.SetActive(true);
    walkCanvas.SetActive(false);
    pivotCanvas.SetActive(false);
    pickupCanvas.SetActive(false);
    throwCanvas.SetActive(false);

    walkCanvas.GetComponent<CanvasGroup>().alpha = 0;
    pivotCanvas.GetComponent<CanvasGroup>().alpha = 0;
    pickupCanvas.GetComponent<CanvasGroup>().alpha = 0;
    throwCanvas.GetComponent<CanvasGroup>().alpha = 0;

    FadeToWalk();
  }

  void Awake () {
    headCanvas = GameObject.Find("Controller-Head");
    walkCanvas = GameObject.Find("Controller-Walk");
    pivotCanvas = GameObject.Find("Controller-Pivot");
    pickupCanvas = GameObject.Find("Controller-PickUp");
    throwCanvas = GameObject.Find("Controller-Throw");
  }
  
  // Update is called once per frame
  void Update () {
    RaycastHit hit = new RaycastHit();
    if ( (Input.GetAxis("Vertical") != 0) && walkCanvas.activeSelf) {
      FadeToPivot();
    }
    if ( (Input.GetAxis("Horizontal") != 0) && pivotCanvas.activeSelf) {
      StartCoroutine(FadeOut(pivotCanvas, 0.05F));
    }
    if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 16)) {
      if (hit.collider.gameObject.tag == "PickUp" && (!carrying) && (!walkCanvas.activeSelf) && (!pivotCanvas.activeSelf) && (!pickupCanvas.activeSelf) && (!throwCanvas.activeSelf))
      {
        FadeToPickup();
      }
    }
    if (pickupCanvas.activeSelf && Input.GetButtonDown("Fire2")) {
      if (carrying) {
        FadeToThrow();
        Debug.Log("show throw");
      }
    }
    if (throwCanvas.activeSelf && (Input.GetButtonDown("Fire2"))) {
      if (!carrying) {
        StartCoroutine(FadeOut(throwCanvas, 0.5F)); 
      }
    }
    Debug.Log(carrying);
  }

  void FadeToWalk () {
    StartCoroutine(WaitFunction(4.0F));
  }

  void FadeToPivot () {
    StartCoroutine(FadeOut(walkCanvas, 0.3F));
    StartCoroutine(FadeIn(pivotCanvas, 0.1F));
  }

  void FadeToPickup () {
    StartCoroutine(FadeIn(pickupCanvas, 0.3F));
    // Debug.Log(carrying);
    // if (pickupCanvas.activeSelf && Input.GetButtonDown("Fire2")) {
    //   if (carrying) {
    //     FadeToThrow();
    //     Debug.Log("show throw");
    //   }
    // }
    // if (throwCanvas.activeSelf && (Input.GetButtonDown("Fire2"))) {
    //   if (!carrying) {
    //     StartCoroutine(FadeOut(throwCanvas, 0.5F)); 
    //   }
    // }
  }

  void FadeToThrow () {
    StartCoroutine(FadeOut(pickupCanvas, 1.0F));
    StartCoroutine(FadeIn(throwCanvas, 0.5F));
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
}