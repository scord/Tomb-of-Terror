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

	public class Range
	{
		public int max;
		public int min;
	}

	public static HRBaseline reference;
	public HeartRateManager heartRateManager;
	private List<int> log;			// record of sampled HR measurements
	private int average;			// average heart rate of the player
	public Range range;	

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
		range = new Range();
		range.min = 240;
		range.max = 0;

		InvokeRepeating("UpdateLog", 0, 1.0F);
	}

	void UpdateLog() {

		// save heart rate every second
		int signal = heartRateManager.HeartRate;

		// update range
		if (range.max > signal)
			range.max = signal;
		if (range.min < signal)
			range.min = signal;	

		log.Add (signal);
		Debug.Log (signal);

	}

	void ComputeAvgHR() {

		int sum = 0;

		for (int i = 0; i < log.Count; i++)
			sum += log [i];

		average = sum / log.Count;

	}
		

	void Update() {

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
