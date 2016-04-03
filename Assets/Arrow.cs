using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	float timer = 0.0f;
	Vector3 origin;
	// Use this for initialization
	void Start () {
		origin = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		transform.position = new Vector3(origin.x, origin.y + Mathf.Sin (timer), origin.z);
	}
}
