using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScreenFlashController : MonoBehaviour {

	[SerializeField] private GameObject redCanvasFront;

	public bool hit = false;
	bool flashing = false;

	void Start() {
		redCanvasFront.SetActive(false);
	}
		

	void Update() {
		if (hit && flashing == false) {
			flashing = true;
			StartCoroutine (FadeIn (redCanvasFront, 6.0f));
		}

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
						
		StartCoroutine (FadeOut (redCanvasFront, 6.0f));
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
		hit = false;
		flashing = false;
		obj.SetActive(false);
	}

	protected void OnDisable() {
		redCanvasFront.SetActive(false);
	}
}