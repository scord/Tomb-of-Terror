using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class ExplorerMusicController : MonoBehaviour {

	private Renderer mummyRenderer;

    AudioSource audio_source;
    bool drama_music_playing = false;
    bool play_drama_music = false;
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
        GameObject background_audio = GameObject.FindGameObjectWithTag("Background_audio");
        AudioSource[] audio_array = background_audio.GetComponents<AudioSource>();
        GameObject mummy = GameObject.FindGameObjectWithTag("Mummy");
        if (mummy != null){
            mummyRenderer = mummy.GetComponentInChildren<Renderer>();
        }
		
		if (mummyRenderer != null && mummyRenderer.IsVisibleFrom(Camera.main)){
			Debug.Log("mummy seen");

		}

        if (gameObject.GetComponent<Player_SyncState>().GetCarryingPrize())
        {
            play_drama_music = true;
        }

        if (!drama_music_playing && play_drama_music)
        {
            audio_array[6].loop = true;
            audio_array[6].Play();
            audio_array[7].loop = true;
            audio_array[7].Play();
            audio_array[8].loop = true;
            audio_array[8].Play();
            drama_music_playing = true;
        }
	}
}
