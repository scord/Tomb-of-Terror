using UnityEngine;
using System.Collections;

public class ExplorerMusicController : MonoBehaviour {

	private Renderer mummyRenderer;

    AudioSource audio_source;

	// Use this for initialization
	void Start () {
        GameObject mummy = GameObject.FindGameObjectWithTag("Mummy");
        GameObject background_audio = GameObject.FindGameObjectWithTag("background_audio");
        AudioSource[] audio_array = background_audio.GetComponents<AudioSource>();

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
		
		if (mummyRenderer != null && mummyRenderer.IsVisibleFrom(Camera.main)){
			Debug.Log("mummy seen");
		}
	}
}
