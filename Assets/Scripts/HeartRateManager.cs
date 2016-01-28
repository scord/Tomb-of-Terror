using UnityEngine;
using System.Collections;
using System.Threading;
using System;

public class HeartRateManager : MonoBehaviour {
	Thread thread;
	bool programActive = true;
	// Use this for initialization
	void Start () {

		object path = Application.dataPath;
		thread = new Thread(new ParameterizedThreadStart(ProcessData));
		thread.Start(path);
	}

	void ProcessData(object ob){
		string path = ob.ToString();

		Debug.Log ("Thread started");

		while (programActive) {

			Debug.Log(path+"/Resources/HR.txt");
			string text = System.IO.File.ReadAllText(path+"/Resources/HR.txt");
			int hr = Convert.ToInt32(text);

			System.Threading.Thread.Sleep(350);
		}
		Debug.Log ("Thread stopped");
	}

	public void OnDisable(){
		Debug.Log ("Heart Rate Disabled");
		programActive = false;   

	}

	// Update is called once per frame
	void Update () {

	}
}