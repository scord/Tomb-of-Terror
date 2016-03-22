using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour
{
	public class Zone 
	{
		public int lower_bound;
		public int upper_bound;
	}

	public static Stats reference;

	// History of the heart rate data //

	public HeartRateManager heartRateManager;
//	public HRBaseline baseline; 
	private int starting_point; 	// presumably the resting state HR of the player
	private int signal;				// incoming signal coming from BTScript every S seconds (sampled heart rate)
	private int[] spikes;			// spikes in the five heart rate zones
	public int max_hr = 200;		// average maximum rate for 20 year-olds
	private Zone[] zones;			// heart rate zones (very light, light, moderate, hard, maximum)
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

	void Start() {

		// initialise all lists & arrays //
		log = new List<int> ();
		partials = new List<int> ();
		times = new List<double> ();
		spikes = new int[5];
		zones = new Zone[5];
		average = 0;

		// Get HeartRate //
		heartRateManager = GameObject.Find ("HeartRate").GetComponent<HeartRateManager> ();
		if (heartRateManager == null)
			Debug.Log ("null heart rate");
		starting_point = heartRateManager.HeartRate;
		signal = starting_point;
		min = starting_point;
		max = starting_point;

		// Load relevant data from Intro Scene // 
//		baseline = GameObject.Find ("HeartRateListener").GetComponent<HRBaseline> ();
//		if (baseline == null)
//			Debug.Log ("null baseline");

		// add first reading to the log//
		log.Add (starting_point);

		// initialise spikes with 0 //
		for (int i = 0; i < 5; i++)
			spikes [i] = 0;

		// calculateMaxHR();
		// calculateZones();

		// start timer for data collection //
		timer = 0.0;

		InvokeRepeating("UpdateLog", 8, 1.0F);	// start collecting HR measurements after 8 seconds; player might be nervous at first
		InvokeRepeating("UpdateAvg", 8, refresh_time);
	}

	void UpdateLog() {

		int prev = signal;

		signal = heartRateManager.HeartRate;
		log.Add (signal);

		if (signal < min)
			min = signal;
		else if (signal > max)
			max = signal;

		if (prev >= signal + relevance) {		// we consider a relevant increase in BPM if it's at least 5 points over the previous measurement
			UpdateSpikes (signal);
			times.Add ((double)timer);
		}


	}

	void UpdateAvg() {

		AverageRate (false);

	}

	// updates incoming signal, minimum/maximum HR measurements, spikes count, timer
	void Update() {

		// TESTING PURPOSES //
		if(Input.GetKey ("l"))
			SceneManager.LoadScene ("Scenes/endgame");
	}

	// compute HR zones
	/*void calculateZones() {

		zones [0].lower_bound = max_hr / 2;
		zones [0].upper_bound = (int) 0.6 * max_hr;

		zones [1].lower_bound = (int) 0.61 * max_hr;
		zones [1].upper_bound = (int) 0.7 * max_hr;

		zones [2].lower_bound = (int) 0.71 * max_hr;
		zones [2].upper_bound = (int) 0.8 * max_hr;

		zones [3].lower_bound = (int) 0.81 * max_hr;
		zones [3].upper_bound = (int) 0.89 * max_hr;

		zones [4].lower_bound = (int) 0.9 * max_hr;
		zones [4].upper_bound = 210;

	}*/

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

	// Clear data log & output statistics
	void onApplicationQuit() {

		log.Clear ();
		partials.Clear ();
		timer = 0.0;

	}
}


