using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public int selected;
    Text[] buttons;
    public NetManager manager;
    string ip;
    public bool host;
	// Use this for initialization
	void Start () {
        selected = 0;
	    buttons = GetComponentsInChildren<Text>();
    }

    bool pressed = false;
	// Update is called once per frame

    public void JoinGame(int playerID)
    {
        ip = transform.FindChild("IPAddressInput").GetComponent<InputField>().text;
        Debug.Log(ip);

        manager.JoinGame(playerID, false, ip);
    }

    public void CreateGame(int playerID)
    {
        ip = transform.FindChild("IPAddressInput").GetComponent<InputField>().text;
        Debug.Log(ip);


        manager.JoinGame(playerID, true, ip);
    }
    void Update () {



	}
}
