using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExplorerTutorialScript : IntroTutorialScript {

  [SerializeField] private GameObject headCanvas, walkCanvas, pivotCanvas, pickupCanvas, throwCanvas, gobackCanvas, runCanvas, extinguishCanvas, toTombCanvas, torchCanvas,enterCanvas;
  [SerializeField] Text fireInfo;
  [SerializeField] private PlayerController playerController;
  [SerializeField] private ExplorerController explorerController;
  [SerializeField] private Camera cam;
  [SerializeField] private GameObject explorerObject;

  // torch variables
  private int torchPress = 0;

  // run variables
  private float runTime = 3;   // seconds
  private bool running = false;

  // pivot variables
  private int pivotCount = 4;

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
    toTombCanvas.SetActive(false);
    enterCanvas.SetActive(false);
    torchCanvas.SetActive(false);

    walkCanvas.GetComponent<CanvasGroup>().alpha = 0;
    pivotCanvas.GetComponent<CanvasGroup>().alpha = 0;
    pickupCanvas.GetComponent<CanvasGroup>().alpha = 0;
    throwCanvas.GetComponent<CanvasGroup>().alpha = 0;
    gobackCanvas.GetComponent<CanvasGroup>().alpha = 0;
    runCanvas.GetComponent<CanvasGroup>().alpha = 0;
    extinguishCanvas.GetComponent<CanvasGroup>().alpha = 0;
    toTombCanvas.GetComponent<CanvasGroup>().alpha = 0;
    enterCanvas.GetComponent<CanvasGroup>().alpha = 0;
    torchCanvas.GetComponent<CanvasGroup>().alpha = 0;

    playerController = explorerObject.GetComponent<PlayerController>();
    explorerController = explorerObject.GetComponent<ExplorerController>();

    FadeToWalk();
  }

  // Update is called once per frame
  protected void Update () {
    fireInfo.enabled = false;
    RaycastHit hit = new RaycastHit();
    if ( (Input.GetAxis("Vertical") != 0) && walkCanvas.activeSelf) {
      FadeTo(walkCanvas, pivotCanvas);
    }
    // if pivot canvas activated
    else if (pivotCanvas.activeSelf) {
      if( EndPivot() )
        FadeTo(pivotCanvas, runCanvas);
    }
    // if the pickup canvas is activated, fade when player is carrying an object
    // else if (pickupCanvas.activeSelf) {
    //   if (playerController.carrying)
    //     FadeTo(pickupCanvas, throwCanvas);
    // }
    // if the throw canvas is activated, fade when player isn't carrying anymore
    // else if (throwCanvas.activeSelf) {
    //   if (Input.GetButtonDown("Fire2")) {
    //     FadeTo(throwCanvas, runCanvas);
    //   }
    // }
    // if run canvas activated
    else if( runCanvas.activeSelf ) {
      if( Input.GetKey(KeyCode.LeftShift) || OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0 )
        running = true;
      else
        running = false;

      if(running && runTime > 0){
        runTime -= Time.deltaTime;
      }

      if(runTime < 0)
        FadeTo(runCanvas, toTombCanvas);

    }
    // if fire pots canvas activated
    // else if( extinguishCanvas.activeSelf ){
    //   if(Input.GetButtonDown("Fire3") ) {
    //     FadeTo(extinguishCanvas, toTombCanvas);
    //   }
    // }
    else if (toTombCanvas.activeSelf ){
      if(explorerController.carryingTorch)
        FadeTo(toTombCanvas, torchCanvas);
    }

    // if at the torch stage
    else if(explorerController.carryingTorch && torchCanvas.activeSelf){

      if(Input.GetButtonDown("Fire1"))
        torchPress ++;

      if(torchPress > 1){
        GameObject.FindGameObjectWithTag("PyramidExit").GetComponent<Collider>().isTrigger = true;
        FadeTo(torchCanvas, enterCanvas);
      }

    }
  }

  private bool EndPivot(){
    if(pivotCount > 0){
      if ( Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q) || OVRInput.Get(OVRInput.Button.PrimaryShoulder) || OVRInput.Get(OVRInput.Button.SecondaryShoulder)  )
        pivotCount--;

      return false;
    }
    return true;
  }

  private void FadeToWalk () {
    StartCoroutine(WaitFunction(4.0F, headCanvas, walkCanvas));
  }

  private void FadeTo( GameObject fromCanvas, GameObject toCanvas){
    StartCoroutine(FadeOut(fromCanvas, 0.5F));
    StartCoroutine(FadeIn(toCanvas, 0.2F));
  }

  IEnumerator WaitFunction (float waitTime, GameObject fromCanvas, GameObject toCanvas)
  {
    yield return new WaitForSeconds(waitTime);
    StartCoroutine(FadeOut(fromCanvas, 0.5F));
    StartCoroutine(FadeIn(toCanvas, 0.2F));
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
    torchCanvas.SetActive(false);
    enterCanvas.SetActive(false);
    toTombCanvas.SetActive(false);
  }
}
