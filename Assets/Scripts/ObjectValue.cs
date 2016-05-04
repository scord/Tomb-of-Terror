using UnityEngine;
using System.Collections;

public class ObjectValue : MonoBehaviour {

  [SerializeField] private int m_ObjectValue = 50; //default
  public int objectValue { get { return m_ObjectValue; }}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
