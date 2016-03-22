using UnityEngine;
using System.Collections;

public class FadeIntroScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	 
	}
	
	// Update is called once per frame
	void Update () {
    if (Input.anyKeyDown) {
      StartCoroutine(FadeIntroCanvas());
    }
	}

  IEnumerator FadeIntroCanvas() {
    CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
    while (canvasGroup.alpha > 0) {
      canvasGroup.alpha -= Time.deltaTime;
      yield return null;
    }

    canvasGroup.interactable = false;
    yield return null;
  }
}
