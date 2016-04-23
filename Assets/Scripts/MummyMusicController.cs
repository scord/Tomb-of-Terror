using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MummyMusicController : MonoBehaviour {

		private Renderer explorerRenderer;

		// Use this for initialization
		void Start () {
        GameObject background_audio = GameObject.FindGameObjectWithTag("Background_audio");
        AudioSource[] audio_array = background_audio.GetComponents<AudioSource>();

        if (SceneManager.GetActiveScene().name == "Sample")
        {
            audio_array[0].loop = true;
            audio_array[0].Play();
        }

        GameObject explorer = GameObject.FindGameObjectWithTag("Explorer");

			if (explorer != null){
				explorerRenderer = explorer.GetComponentInChildren<Renderer>();
			}
		}

		// Update is called once per frame
		void Update () {

			GameObject explorer = GameObject.FindGameObjectWithTag("Explorer");

			if (explorer != null){
				explorerRenderer = explorer.GetComponentInChildren<Renderer>();
			}

			if (explorerRenderer != null && explorerRenderer.IsVisibleFrom(Camera.main)){
				Debug.Log("explorer seen");
			}
		}
	}
