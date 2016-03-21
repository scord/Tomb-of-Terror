using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class HRBaseline : MonoBehaviour {

	public class Relevance
	{
		public int time;		// time over which a spike appears
		public int num_spikes;	// how many points should HR increase to be considered spike
		public float stddev_spikes;
	}

	public static HRBaseline reference;
	public HeartRateManager heartRateManager;
	private List<int> log;			// record of sampled HR measurements
	private int average;			// average heart rate of the player
	public Relevance parameters;	// parameters to be computed & to be used later on
	double timer;					// time in seconds

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

		heartRateManager = GameObject.Find ("HeartRate").GetComponent<HeartRateManager> ();
		if (heartRateManager == null)
			Debug.Log ("null heart rate");

		// initialise all lists & arrays //
		log = new List<int> ();
		average = 0;
		parameters = new Relevance();

		// start timer for data collection //
		timer = 0.0;

		InvokeRepeating("UpdateLog", 0, 1.0F);
	}

	void UpdateLog() {

		// save heart rate every second
		log.Add (heartRateManager.HeartRate);
		Debug.Log (heartRateManager.HeartRate);

	}

	void ComputeAvgHR() {

		int sum = 0;

		for (int i = 0; i < log.Count; i++)
			sum += log [i];

		average = sum / log.Count;

	}

	// Function to interpret HR pattern // 
	void ComputeRelevance() {

		List<int> interesting_times = new List<int> (); // in seconds
		List<int> differences = new List<int>();
		int sum = 0;

		// int start = log [0];

		// Go through HR log & look for spikes//
		for (int i = 1; i < log.Count; i++) {

			if (log [i] > log [i - 1]) {

				interesting_times.Add (i);
				differences.Add (log[i] - log[i-1]);

			}
		}

		// Compute average difference ? std. dev?
		for (int i = 0; i < differences.Count; i++) {

			sum += differences [i];

		}

		parameters.num_spikes = sum / differences.Count;
		parameters.stddev_spikes = std_dev(differences, parameters.num_spikes);

		// Compute average spike time? std. dev?
		for (int i = 0; i < interesting_times.Count; i++) {



		}

	}

	float std_dev(List<int> data, int miu) {

		float s = 0;

		for (int i = 0; i < data.Count; i++) {

			s += (data [i] - miu) * (data [i] - miu);

		}

		return (float)Math.Sqrt(s / data.Count);

	}
		

	void Update() {

		// Update timer //
		timer += Time.deltaTime;

		// TESTING PURPOSES //
		if (Input.GetKey ("h")) {
			
			ComputeAvgHR ();
			Debug.Log (average);

		}

		// Queue to load the main scene //
		/*
		if(moving_on) { 

			ComputeAvgHR();
			ComputeRelevance(); // spiking pattern (no. of points increased in HR & over what time span)
			SceneManager.LoadScene ("Scenes/main"); // possibly

		}


		*/

	}

}
