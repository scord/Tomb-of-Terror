using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DrawDistanceFix : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Camera cam = GetComponent<Camera>();
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("startscene"))
            cam.farClipPlane = 400;
        else cam.farClipPlane = 150;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
