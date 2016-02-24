using UnityEngine;
using System.Collections;

public class FinalData : MonoBehaviour {

	void OnGui() {
		
		if(GUI.Button (new Rect(10, 180, 100, 30), "Signal "))
			Stats.signal += 5;
	}
}
