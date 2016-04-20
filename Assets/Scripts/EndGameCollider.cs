using UnityEngine;
using System.Collections;

public class EndGameCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something at maze exit");
        if (other.gameObject.tag == "Explorer")
        {
            Debug.Log("Explorer at entrance");
			//SceneManager.LoadScene ("Scenes/endgame");
        }
    }
}
