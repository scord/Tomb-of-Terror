using UnityEngine;
using System.Collections;

public class SoundVision : MonoBehaviour {
	
	public Shader shader;
    public AudioSource audioSource;
    public int n = 12; //number of possible simultaneous waves
    float[] time;
    bool[] active;
    int count = 0;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
		gameObject.GetComponent<Camera>().SetReplacementShader(shader, "");

        time = new float[n];
        active = new bool[n];

        Shader.SetGlobalFloat("_N", n);

        for (int i = 0; i < n; i++)
        {
            Shader.SetGlobalVector("_SoundSource" + i, new Vector3(0.0f, 0.0f, 0.0f));
            time[i] = 0.0f;
            active[i] = false;
        }

    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButtonDown("Fire1"))
        {
            time[count] = 0;
            active[count] = true;
            Shader.SetGlobalVector("_SoundSource" + count, transform.position);
            count = (count + 1) % 12;
            audioSource.time = 0.5f;
            audioSource.Play();
        }

        for (int i = 0; i < 12; i++)
        {
            if (active[i])
            {
                time[i] += Time.deltaTime;
               
            }
            Shader.SetGlobalVector("_Colors" + i, new Vector4(0.5f, 1.0f, 1.0f, time[i]));
        }

        

        
	}
	
	void OnPostRender() {

	}


}
