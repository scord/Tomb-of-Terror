using UnityEngine;
using System.Collections;

public class TimerBlinkingScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
    StartCoroutine(Blinking());	
	}

  IEnumerator Blinking () {
    Debug.Log("blink");
    float increment;
    CanvasGroup cv = GetComponent<CanvasGroup>();
    float speed = 1.0F;
    while (true) {
      // fade out
      while (cv.alpha > 0.4) {
          increment = speed * Time.deltaTime;
          if (cv.alpha - increment < 0) cv.alpha = 0;
          else cv.alpha -= speed * Time.deltaTime;
          yield return null;
      }
      //
      while (cv.alpha < 1) {
          increment = speed * Time.deltaTime;
          if (cv.alpha + increment > 1) cv.alpha = 1;
          else cv.alpha += speed * Time.deltaTime;
          yield return null;
      }
    }
  }
	
	// Update is called once per frame
	void Update () {
    // StartCoroutine(Blinking());
	}
}
