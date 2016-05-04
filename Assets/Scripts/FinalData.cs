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

			if (my_stats.GetMin () != my_stats.GetMax ())
				range.text = my_stats.GetMin ().ToString () + " - " + my_stats.GetMax ().ToString ();
			else
				range.text = "You had a constant heart rate of " +  my_stats.GetAverage ().ToString() + ".";

			start_point.text = my_stats.GetStartPoint().ToString();

			log = my_stats.GetLog ();
			times = my_stats.GetTimes ();

			if (times.Count == 0) {

				times_placeholder = "Your heart rate didn't spike more than " + my_stats.relevance.ToString () + " points in a second.";
			}

			else {

				times_placeholder = turnToString (times);
				times_text.text = times_placeholder;

			}

		}
	}

	string turnToString(List<double> input) {

		string result = "";
		int i,min,sec;

		for (i = 0; i < input.Count - 1; i++) {

			min = (int)(input [i] / 60);
			sec = (int)(input [i] % 60);
			result += min.ToString () + "m " + sec.ToString () + "s" + "; ";	// 3m 2s; 5m 10s
		}

		result += input [i].ToString ();

		return result;

	}


}
