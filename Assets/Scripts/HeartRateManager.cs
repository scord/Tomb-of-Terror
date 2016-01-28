using UnityEngine;
using System.Collections;
using System.Threading;
using System;
using UnityEngine.Networking;

public class HeartRateManager : NetworkBehaviour {
	Thread thread;
	bool programActive = true;

	System.Diagnostics.Process HRProcess;

	[SyncVar]
	public int HeartRate;
	
	// Use this for initialization
	void Start () {

		object path = Application.dataPath;
		
		HRProcess = new System.Diagnostics.Process();
		HRProcess.StartInfo.FileName = path+"/Scripts/dist/bglib_test_hr_collector.exe";
 
        HRProcess.StartInfo.RedirectStandardOutput = true;
		HRProcess.StartInfo.UseShellExecute = true;
		HRProcess.StartInfo.CreateNoWindow = true;
		HRProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		
		HRProcess.Start();
		
		thread = new Thread(new ParameterizedThreadStart(ProcessData));
		thread.Start(path);
	}

	void ProcessData(object ob){
		string path = ob.ToString();

		Debug.Log ("Thread started");

		while (programActive) {
			Debug.Log("before");
			string text = HRProcess.StandardOutput.ReadLine();
			HeartRate = Convert.ToInt32(text);
			Debug.Log("after "+HeartRate);
			System.Threading.Thread.Sleep(350);
		}
		Debug.Log ("Thread stopped");
	}

	public void OnDisable(){
		HRProcess.CloseMainWindow();
            // Free resources associated with process.
        HRProcess.Close();
		Debug.Log ("Heart Rate Disabled");
		programActive = false;   

	}

	 
	void Update () {

	}
}