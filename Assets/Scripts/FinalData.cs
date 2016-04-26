using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FinalData : MonoBehaviour {

	public HeartRateManager my_stats;

	public Text average_hr;
	public Text range;
	public Text start_point;
	public Text times_text;

	private List<int> log;
	private List<double> times;

	void Start() {
		GameObject go = GameObject.Find("HeartRate");
		string times_placeholder = "";

		if (go != null) {
			my_stats = go.GetComponent<HeartRateManager> ();
			average_hr.text = my_stats.GetAverage ().ToString();
			range.text = my_stats.GetMin().ToString() + " - " + my_stats.GetMax().ToString();
			start_point.text = my_stats.GetStartPoint().ToString();

			log = my_stats.GetLog ();
			times = my_stats.GetTimes ();

			if (times.Count == 0) {

				times_placeholder = "Your heart rate didn't spike more than " + my_stats.relevance.ToString () + " points in a second";
			}

			else {

				times_placeholder = turnToString (times);
				times_text.text = times_placeholder;

			}

		}
	}

	string turnToString(List<double> input) {

		string result = "";
		int i;

		for (i = 0; i < input.Count - 1; i++) {

			result += input [i].ToString () + "; ";
		}

		result += input [i].ToString ();

		return result;

	}


}
