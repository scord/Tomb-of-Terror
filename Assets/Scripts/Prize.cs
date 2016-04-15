using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Prize : MonoBehaviour {
    bool showText = false;
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Explorer")
        {
            showText = true;
            //currently menu is set to secene 0;
            //Application.LoadLevel(0);
			SceneManager.LoadScene ("Scenes/endgame");
        }
    }

    void OnGUI()
    {

        if (showText)
        {
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Explorer wins!");
        }

    }

}
