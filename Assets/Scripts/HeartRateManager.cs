using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class HeartRateManager : NetworkBehaviour {
	Thread thread;
	bool programActive = true;

    public static HeartRateManager reference;
    
    
	System.Diagnostics.Process HRProcess;
    System.Diagnostics.Process bluKillProcess;

  public delegate void HRDelegate(int newHR);
  public event HRDelegate EventHRUpdate;

	//[SyncVar]
	public int HeartRate;
    
    // Stats
    
    //	public HRBaseline baseline; 
	private int starting_point; 	// presumably the resting state HR of the player
	private int signal;				// incoming signal coming from BTScript every S seconds (sampled heart rate)
	private int[] spikes;			// spikes in the five heart rate zones
	public int max_hr = 200;		// average maximum rate for 20 year-olds
	private List<int> log;			// record of sampled HR measurements
	private List<int> partials; 	// record of partial average HR computed along the way
	private int max, min; 			// variables to hold minimum and maximum BPM
	private int average;

	// PARAMETERS //
	public int relevance = 5;		// relevant spike iff new signal is at least 5 points above the previous signal
	public float refresh_time = 300.0F;  // compute average HR so far - every 5 minutes (300 seconds)

	double timer;					// time in seconds
	private List<double> times; 	// times at which spikes occurred

	// Make sure there is only one instance of this objects (stats), and that it's in every scene. 
	// data persistence reference code: http://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/persistence-data-saving-loading
	void Awake() {
		if (reference == null) {
			DontDestroyOnLoad (gameObject); // keep it in every scene
			reference = this;
		} 
		else if (reference != this) {
			Destroy (gameObject);
		}
	}
    
    
    
    
	
	// Use this for initialization
	void Start () {
        
        
        //stats
        
        // initialise all lists & arrays //
		log = new List<int> ();
		partials = new List<int> ();
		times = new List<double> ();
		spikes = new int[5];
		average = 0;

		// Load relevant data from Intro Scene // 
//		baseline = GameObject.Find ("HeartRateListener").GetComponent<HRBaseline> ();
//		if (baseline == null)
//			Debug.Log ("null baseline");


		// initialise spikes with 0 //
		for (int i = 0; i < 5; i++)
			spikes [i] = 0;

		// calculateMaxHR();
		// calculateZones();

		// start timer for data collection //
		timer = 0.0;

		InvokeRepeating("UpdateLog", 8, 1.0F);	// start collecting HR measurements after 8 seconds; player might be nervous at first
		InvokeRepeating("UpdateAvg", 8, refresh_time);
        
        //bluetooth

		object path = Application.streamingAssetsPath;
		
		HRProcess = new System.Diagnostics.Process();
		HRProcess.StartInfo.FileName = path + "\\HR\\bglib_test_hr_collector.exe";
 		Debug.Log(path);
		HRProcess.StartInfo.UseShellExecute = false;
		HRProcess.StartInfo.RedirectStandardOutput = true;
		HRProcess.StartInfo.CreateNoWindow = true;
		HRProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		
        
        bluKillProcess = new System.Diagnostics.Process();
		bluKillProcess.StartInfo.FileName = "C:\\Windows\\system32\\cmd.exe";
    bluKillProcess.StartInfo.Arguments = "/c" + path + "\\HR\\bglibKill.bat";
		bluKillProcess.StartInfo.UseShellExecute = false;
		bluKillProcess.StartInfo.RedirectStandardOutput = true;
		bluKillProcess.StartInfo.CreateNoWindow = true;
		bluKillProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        
		HRProcess.Start();
		
		thread = new Thread(new ParameterizedThreadStart(ProcessData));
		thread.Start(path);

		starting_point = HeartRate;
		signal = starting_point;
		min = starting_point;
		max = starting_point;
		// min = 300; max = 0; //

		// add first reading to the log//
		log.Add (starting_point);
	}
    
    void UpdateLog() {

		int prev = signal;

		signal = HeartRate;
		log.Add (signal);

		if (signal < min)
			min = signal;
		else if (signal > max)
			max = signal;

		if (prev >= signal + relevance) {		// we consider a relevant increase in BPM if it's at least 5 points over the previous measurement
			//UpdateSpikes (signal);
			times.Add ((double)timer);
		}


	}

	void UpdateAvg() {

		AverageRate (false);

	}

	void ProcessData(object ob){
		//string path = ob.ToString();

		Debug.Log ("Thread started " + ob);
    String text;
		while (programActive) {
      text = HRProcess.StandardOutput.ReadLine();
			HeartRate = Convert.ToInt32(text);
		}
		Debug.Log ("Thread stopped");
	}
    /*
    	// update record of HR spikes when we discover a sudden change in BPM
	void UpdateSpikes(int signal) {

		if (signal <= zones [0].upper_bound && signal >= zones [0].lower_bound)
			spikes [0]++;
		else if (signal <= zones [1].upper_bound && signal >= zones [1].lower_bound)
			spikes [1]++;
		else if (signal <= zones [2].upper_bound && signal >= zones [2].lower_bound)
			spikes [2]++;
		else if (signal <= zones [3].upper_bound && signal >= zones [3].lower_bound)
			spikes [3]++;
		else spikes [4]++;

	}*/

	public void OnDisable(){
        
    log.Clear();
		partials.Clear();
		timer = 0.0;
            // Free resources associated with process.
        programActive = false;
        
        bool stopped = false;
        bluKillProcess.Start();
        Debug.Log(bluKillProcess.StartInfo.Arguments);
        while(!stopped){
            String output = bluKillProcess.StandardOutput.ReadLine();
            stopped = output.Equals("done!");
        }
        thread.Abort();
        bluKillProcess.Kill();
        bluKillProcess.WaitForExit();
        
		Debug.Log ("Heart Rate Disabled");
	}
    
    // average rate throughout the game
	// to avoid overflow: hold HR information for a set amount of time and calculate its average, storing it in a variable;
	// clear the contents of the log and do the same until the game ends / players quit 
	void AverageRate (bool flag) {

		int avg = 0;

		// check if the the call requests the final value
		if (flag == false) {

			for (int i = 0; i < log.Count; i++)
				avg += log [i];

			avg = avg / log.Count;
			partials.Add (avg);

			log.Clear ();
		} 
		else {

			for (int i = 0; i < partials.Count; i++)
				avg += partials [i];

			average = avg / partials.Count;

			Console.WriteLine("Average heart rate through the game: ", average);

		}

	}

	public int GetStartPoint() {
		return starting_point;
	}

	public int GetAverage() {
		AverageRate (true);
		return average;
	}

	// Range
	public int GetMin() {
		return min;
	}
	public int GetMax() {
		return max;
	}

	// Function to access the log of HR measurements saved
	public List<int> GetLog() {
		return log;
	}

	// Function to access the current HR measurement
	public int GetHR() {
		return signal;
	}

	// Function to access the times when a spike in the HR occurred
	public List<double> GetTimes() {
		return times;
	}


	 
	void Update () {

		int avg = 0; 

		if (EventHRUpdate != null) {
			EventHRUpdate(HeartRate);
		}

		if (Input.GetKey ("l")) {

			for (int i = 0; i < log.Count; i++)
				avg += log [i];

			avg = avg / log.Count;
			partials.Add (avg);

			AverageRate(true);	// compute *final* average hr
			SceneManager.LoadScene ("Scenes/endgame");

		}

		timer += Time.deltaTime;
	}
}