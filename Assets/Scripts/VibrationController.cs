using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class VibrationController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void VibrateFor(float seconds) {
    GamePad.SetVibration(0, 1.0f, 1.0f);
    StartCoroutine(StopVibrationDelayed(seconds));
  }

  void OnDisable() {
    StopVibration();
  }

  public void StopVibration() {
    GamePad.SetVibration(0, 0.0f, 0.0f);
  }

  IEnumerator StopVibrationDelayed(float seconds) {
    yield return new WaitForSeconds(seconds);
    StopVibration();
  }
}
