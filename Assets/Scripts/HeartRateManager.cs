using UnityEngine;
using System.Collections;
using System.Threading;
using System;
using UnityEngine.Networking;

public class HeartRateManager : NetworkBehaviour {
	Thread thread;
	bool programActive = true;

	System.Diagnostics.Process HRProcess;
    System.Diagnostics.Process bluKillProcess;

	//[SyncVar]
	public int HeartRate;
	
	// Use this for initialization
	void Start () {

		object path = Application.dataPath;
		
		HRProcess = new System.Diagnostics.Process();
		HRProcess.StartInfo.FileName = path+"/Scripts/HR/bglib_test_hr_collector.exe";
 
		HRProcess.StartInfo.UseShellExecute = false;
		HRProcess.StartInfo.RedirectStandardOutput = true;
		HRProcess.StartInfo.CreateNoWindow = true;
		HRProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		
        
        bluKillProcess = new System.Diagnostics.Process();
		bluKillProcess.StartInfo.FileName = "C:\\Windows\\system32\\cmd.exe";
        bluKillProcess.StartInfo.Arguments = "/c" + path+"/Scripts/HR/bglibKill.bat";
		bluKillProcess.StartInfo.UseShellExecute = false;
		bluKillProcess.StartInfo.RedirectStandardOutput = true;
		bluKillProcess.StartInfo.CreateNoWindow = true;
		bluKillProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        
		HRProcess.Start();
		
		thread = new Thread(new ParameterizedThreadStart(ProcessData));
		thread.Start(path);
	}

	void ProcessData(object ob){
		string path = ob.ToString();

		Debug.Log ("Thread started");
        String text;
		while (programActive) {
            text = HRProcess.StandardOutput.ReadLine();
			HeartRate = Convert.ToInt32(text);
		}
		Debug.Log ("Thread stopped");
	}

	public void OnDisable(){
            // Free resources associated with process.
        programActive = false;
        
        bool stopped = false;
        bluKillProcess.Start();
        while(!stopped){
            String output = bluKillProcess.StandardOutput.ReadLine();
            stopped = output.Equals("done!");
            
        }
        bluKillProcess.Kill();
        bluKillProcess.WaitForExit();
        
		Debug.Log ("Heart Rate Disabled");
  

	}

	 
	void Update () {

	}
}