using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FinalData : MonoBehaviour {

	public Stats my_stats;

	public Text average_hr;
	public Text range;
	public Text start_point;
	public Text times_text;

	private List<int> log;
	private List<double> times;

	void Start() {

		my_stats = GameObject.Find ("HeartRateStats").GetComponent<Stats> ();
		average_hr.text = my_stats.GetAverage ().ToString ();
		range.text = my_stats.GetMin().ToString() + " - " + my_stats.GetMax().ToString();
		start_point.text = my_stats.GetStartPoint().ToString();
		log = my_stats.GetLog ();
		times = my_stats.GetTimes ();
	}


}
