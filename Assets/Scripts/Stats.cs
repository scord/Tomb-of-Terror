using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;

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
	private int starting_point; 	// presumably the resting state HR of the player
	public int signal;				// incoming signal coming from BTScript every S seconds (sampled heart rate)
	private int[] spikes;			// spikes in the five heart rate zones
	public int max_hr = 200;		// average maximum rate for 20 year-olds
	private Zone[] zones;			// heart rate zones (very light, light, moderate, hard, maximum)
	private List<int> log;			// record of sampled HR measurements
	private List<int> partials; 	// record of partial average HR computed along the way
	private int max = 0, min = 240; // variables to hold minimum and maximum BPM

	// PARAMETERS //
	public int relevance = 5;		// relevant spike iff new signal is at least 5 points above the previous signal
	public int refresh_time = 300;  // compute average HR so far - every 5 minutes (300 seconds)

	//private int[] benchmarks; 	// HR measurements taken during intro

	double timer;					// time in seconds
	private List<double> times; 	// times at which spikes occurred
	//public int age;				// user input relevant for computing personalised maximum HR

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

		// get initial starting_point from the first read in BTScript - TBC //
		heartRateManager = GameObject.Find ("HeartRate").GetComponent<HeartRateManager> ();
		starting_point = heartRateManager.HeartRate;
		signal = starting_point;

		// add first reading to the log//
		log.Add (starting_point);

		// fetch benchmark values - TBC //
		// benchmarks[0] = myObject.GetComponent<MyScript>().MyFunction();

		// initialise spikes with 0 //
		for (int i = 0; i < 5; i++)
			spikes [i] = 0;

		// calculateMaxHR();
		// calculateZones();

		// start timer for data collection //
		timer = 0.0;
	}

	// updates incoming signal, minimum/maximum HR measurements, spikes count, timer
	void Update() {

		int prev = signal; 

		// update incoming signal with data from BTScript, then update record of spikes (if necessary) --> read every 4 seconds
		// if(timer % 4 == 0)
		{
			signal = heartRateManager.HeartRate;
		}

		log.Add(signal);

		if (signal < min)
			min = signal;
		else if (signal > max)
			max = signal;

		if (prev >= signal + relevance) {		// we consider a relevant increase in BPM if it's at least 5 points over the previous measurement
			UpdateSpikes (signal);
			times.Add ((double)timer);
		}

		// update timer //
		timer += Time.deltaTime;
		// precompute a partial average rate so far to avoid later overflow --> every 5 minutes//
		if(timer % refresh_time == 0)
			AverageRate(false);
	}

	// calculate HR based on user input - age
	//  void calculateMaxHR() {
	//	  max_hr = 206.9 - 0.67 * age;
	//  }

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

		// check if the the call comes from ApplicationQuit
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

			avg = avg / partials.Count;

			Console.WriteLine("Average heart rate through the game: ", avg);

		}

	}

	// Print range of HR values reached throughout the game
	void GetRange() {

		Console.WriteLine ("Your starting point was: %d", starting_point);
		Console.WriteLine ("Minimum reached: %d", min);
		Console.WriteLine ("Maximum reached: %d", max);

	}

	// Function to access the log of HR measurements saved
	List<int> GetLog() {
		return log;
	}

	// Function to access the current HR measurement
	int GetHR() {
		return signal;
	}

	// Function to access the times when a spike in the HR occurred
	List<double> GetTimes() {
		return times;
	}

	void OnGui() {

		GUI.Label (new Rect (10, 10, 100, 30), "Signal " + signal);

	}

	// Clear data log & output statistics
	void onApplicationQuit() {

		AverageRate(true);
		Range ();
		log.Clear ();
		partials.Clear ();
		timer = 0.0;

	}
}


