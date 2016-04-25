using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExplorerTutorialScript : IntroTutorialScript {

  [SerializeField] private GameObject headCanvas, walkCanvas, pivotCanvas, pickupCanvas, throwCanvas, gobackCanvas, runCanvas, extinguishCanvas;
  [SerializeField] private PlayerController playerController;
  [SerializeField] private Camera cam;
  [SerializeField] private GameObject explorerObject;

  private int pivotCount = 5;
  // Use this for initialization
  protected void Start () {
    headCanvas.SetActive(true);
    walkCanvas.SetActive(false);
    pivotCanvas.SetActive(false);
    pickupCanvas.SetActive(false);
    throwCanvas.SetActive(false);
    gobackCanvas.SetActive(false);
    runCanvas.SetActive(false);
    extinguishCanvas.SetActive(false);

    walkCanvas.GetComponent<CanvasGroup>().alpha = 0;
    pivotCanvas.GetComponent<CanvasGroup>().alpha = 0;
    pickupCanvas.GetComponent<CanvasGroup>().alpha = 0;
    throwCanvas.GetComponent<CanvasGroup>().alpha = 0;
    gobackCanvas.GetComponent<CanvasGroup>().alpha = 0;
    runCanvas.GetComponent<CanvasGroup>().alpha = 0;
    extinguishCanvas.GetComponent<CanvasGroup>().alpha = 0;

    FadeToWalk();
  }

  // Update is called once per frame
  protected void Update () {
    RaycastHit hit = new RaycastHit();
    if ( (Input.GetAxis("Vertical") != 0) && walkCanvas.activeSelf) {
      FadeTo(walkCanvas, pivotCanvas);
    }
    // if pivot canas activated
    else if (pivotCanvas.activeSelf) {
      if( EndPivot() )
        FadeTo(pivotCanvas, runCanvas);
    }
    else if( runCanvas.activeSelf ) {
      if( Input.GetButtonDown("Fire2") ) {
        FadeTo(runCanvas, extinguishCanvas);
      }
    }
    else if( extinguishCanvas.activeSelf ){
      if(Input.GetButtonDown("Fire2") ) {
        StartCoroutine(FadeOut(extinguishCanvas, 1F));
      }
    }
    // if pickupCanvas activated
    // else if(pickupCanvas.activeSelf) {
    //   if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 16)) {
    //     Debug.Log(hit.collider.gameObject.tag);
    //     if (hit.collider.gameObject.tag == "Prize"){
    //       Debug.Log("right object in froont");
    //       if(Input.GetButtonDown("Fire2")){
    //         Debug.Log("picked up");
    //         StartCoroutine(FadeOut(pickupCanvas, 0.05F));
    //       }
    //     }
    //   }
    // }



      // if (hit.collider.gameObject.tag == "PickUp" && (!walkCanvas.activeSelf) && (!pivotCanvas.activeSelf) && (!pickupCanvas.activeSelf) && (!throwCanvas.activeSelf)){
      //   FadeToPickup();
      // }
    // else if (pickupCanvas.activeSelf && playerController.carrying) {
    //   FadeToThrow();
    // }
    // else if (throwCanvas.activeSelf && (!playerController.carrying)) {
    //   StartCoroutine(FadeOut(throwCanvas, 0.5F));
    // }
    else if (explorerObject.transform.position.z < -140 || explorerObject.transform.position.x < -45 || explorerObject.transform.position.x > 45) {
      StartCoroutine(FadeIn(gobackCanvas, 1.0F));
    }
    else if ((explorerObject.transform.position.z > -140 && explorerObject.transform.position.x > -45 && explorerObject.transform.position.x < 45) && gobackCanvas.activeSelf) {
      StartCoroutine(FadeOut(gobackCanvas, 1.0F));
    }
  }

  private bool EndPivot(){
    Debug.Log(pivotCount);
    if(pivotCount > 0){
      if ( Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q) ){
        Debug.Log("pressed");
        pivotCount--;
      }
      return false;
    }
    return true;
  }

  private bool EndPickup(){
    return true;
  }

  private void FadeToWalk () {
    StartCoroutine(WaitFunction(4.0F));
  }

  private void FadeTo( GameObject fromCanvas, GameObject toCanvas){
    StartCoroutine(FadeOut(fromCanvas, 0.5F));
    StartCoroutine(FadeIn(toCanvas, 0.5F));
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

  protected void OnDisable() {
    headCanvas.SetActive(false);
    walkCanvas.SetActive(false);
    pivotCanvas.SetActive(false);
    pickupCanvas.SetActive(false);
    throwCanvas.SetActive(false);
    gobackCanvas.SetActive(false);
    runCanvas.SetActive(false);
    extinguishCanvas.SetActive(false);
  }
}
