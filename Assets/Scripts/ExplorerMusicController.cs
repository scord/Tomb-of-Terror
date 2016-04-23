using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class ExplorerMusicController : MonoBehaviour {

	private Renderer mummyRenderer;

    AudioSource audio_source;

	// Use this for initialization
	void Start () {
        GameObject mummy = GameObject.FindGameObjectWithTag("Mummy");
        GameObject background_audio = GameObject.FindGameObjectWithTag("Background_audio");
        AudioSource[] audio_array = background_audio.GetComponents<AudioSource>();
        //audio_array[5].volume = 1.0f;
        if (SceneManager.GetActiveScene().name == "Sample")
        {
            audio_array[5].loop = true;
            audio_array[5].Play();
        }
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
