using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

    public float t = 0;
    public float m = 1;
    Vector3 origin;
	// Use this for initialization
	void Start () {
        origin = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        t+=0.01f*m;
        m += 0.001f;
        transform.position = origin + new Vector3(Mathf.Sin(t)*7, 0, Mathf.Cos(t)*2);

	}
}
