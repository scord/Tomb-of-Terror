using UnityEngine;
using System.Collections;


public class MummyController : PlayerController {

    string message = "Mummy Wins";
    bool showText = false;

    protected override void Start(){
		base.Start();
	}	


	protected override void Update(){
		base.Update();
	    if (Input.GetButtonDown("Fire1")){
            gameObject.GetComponent<AudioSource>().Play();
            soundVision.EchoLocate();
        }
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Explorer")
        {
            //mummy wins
            showText = true;
            //wait a few seconds
            Application.LoadLevel(0);
        }
    }

    void OnGUI()
    {

        if (showText)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200f, 200f), message);
        }

    }
}