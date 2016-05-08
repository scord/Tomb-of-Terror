using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DisableLight : MonoBehaviour {

    public GameObject light;
	// Use this for initialization
	void Start () {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("startscene"))
            light.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
