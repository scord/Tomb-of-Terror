using UnityEngine;
using System.Collections;

public class ExplorerMusicController : MonoBehaviour {

	private Renderer mummyRenderer;
	
	
	// Use this for initialization
	void Start () {
		mummyRenderer = GameObject.FindGameObjectWithTag("Mummy").GetComponentInChildren<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (mummyRenderer == null){
			mummyRenderer = GameObject.FindGameObjectWithTag("Mummy").GetComponentInChildren<Renderer>();
		}
		
		if (mummyRenderer.IsVisibleFrom(Camera.main)){
			Debug.Log("mummy seen");
		}
	}
}
