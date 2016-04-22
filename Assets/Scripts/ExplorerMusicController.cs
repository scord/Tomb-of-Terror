﻿using UnityEngine;
using System.Collections;

public class ExplorerMusicController : MonoBehaviour {

	private Renderer mummyRenderer;
	
	
	// Use this for initialization
	void Start () {
        GameObject mummy = GameObject.FindGameObjectWithTag("Mummy");
        if (mummy != null){
            mummyRenderer = mummy.GetComponentInChildren<Renderer>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
        GameObject mummy = GameObject.FindGameObjectWithTag("Mummy");
        if (mummy != null){
            mummyRenderer = mummy.GetComponentInChildren<Renderer>();
        }
		
		if (mmummyRenderer != null && mummyRenderer.IsVisibleFrom(Camera.main)){
			Debug.Log("mummy seen");
		}
	}
}
