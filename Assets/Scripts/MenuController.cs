using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public int selected;
    Text[] buttons;
    public NetManager manager;

	// Use this for initialization
	void Start () {
        selected = 0;
	    buttons = GetComponentsInChildren<Text>();
    }

    bool pressed = false;
	// Update is called once per frame
	void Update () {


        if (Input.GetButtonDown("Fire1"))
        {
            if (selected == 1)
            {
                
                manager.JoinGame(1);
            }
            else if (selected == 2)
            {
                Debug.Log("Player 2 Joining");
                manager.JoinGame(2);
            }
            else if (selected == 0)
            {
                Debug.Log("Host Joining");
                manager.CreateGame();
            }
        }
        if (!pressed && Input.GetAxis("Vertical") > 0)
        {
            buttons[selected].color = new Color(0, 0, 0);
            selected = (selected + 1) % buttons.Length;
            pressed = true;
        }

        if (Input.GetAxis("Vertical") == 0)
        {
            pressed = false;
        }

        
        buttons[selected].color = new Color(1, 1, 1);


	}
}
