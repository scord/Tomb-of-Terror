using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroTutorialScript : MonoBehaviour {

  private GameObject headCanvas, walkCanvas, pivotCanvas, pickupCanvas, throwCanvas;
  private PlayerController playerController;
  public Camera cam;
  public GameObject explorerObject;

  // Use this for initialization
  protected void Start () {
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

  protected void Awake () {
    playerController = explorerObject.GetComponent<PlayerController>();
    headCanvas = GameObject.Find("Controller-Head");
    walkCanvas = GameObject.Find("Controller-Walk");
    pivotCanvas = GameObject.Find("Controller-Pivot");
    pickupCanvas = GameObject.Find("Controller-PickUp");
    throwCanvas = GameObject.Find("Controller-Throw");
  }
  
  // Update is called once per frame
  protected void Update () {
    RaycastHit hit = new RaycastHit();
    if ( (Input.GetAxis("Vertical") != 0) && walkCanvas.activeSelf) {
      FadeToPivot();
    }
    else if ( (Input.GetAxis("Horizontal") != 0) && pivotCanvas.activeSelf) {
      StartCoroutine(FadeOut(pivotCanvas, 0.05F));
    }
    else if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 10)) {
      if (hit.collider.gameObject.tag == "PickUp" && (!walkCanvas.activeSelf) && (!pivotCanvas.activeSelf) && (!pickupCanvas.activeSelf) && (!throwCanvas.activeSelf))
      {
        FadeToPickup();
      }
    }
    else if (pickupCanvas.activeSelf && playerController.carrying) {
      Debug.Log("throw");
      FadeToThrow();
    }
    else if (throwCanvas.activeSelf && (!playerController.carrying)) {
      StartCoroutine(FadeOut(throwCanvas, 0.5F)); 
    }
  }

  private void FadeToWalk () {
    StartCoroutine(WaitFunction(4.0F));
  }

  private void FadeToPivot () {
    StartCoroutine(FadeOut(walkCanvas, 0.3F));
    StartCoroutine(FadeIn(pivotCanvas, 0.1F));
  }

  private void FadeToPickup () {
    StartCoroutine(FadeIn(pickupCanvas, 0.3F));
  }

  private void FadeToThrow () {
    StartCoroutine(FadeOut(pickupCanvas, 3.0F));
    StartCoroutine(FadeIn(throwCanvas, 2.0F));
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