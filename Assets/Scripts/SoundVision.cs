using UnityEngine;
using System.Collections;

public class SoundVision : MonoBehaviour {
	
	public Shader shader;
    float[] time = new float[3];
    int count = 0;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
		gameObject.GetComponent<Camera>().SetReplacementShader(shader, "");
		//material.SetFloat ("speed", 50.0f);
		
		Shader.SetGlobalColor ("_BaseColor", new Color (1.0f, 1.0f, 1.0f, 1.0f));
		Shader.SetGlobalVector ("_SoundSource", new Vector3 (6.0f, 3.0f, -10.0f));
		Shader.SetGlobalVector ("_SoundSources0", new Vector3 (3.0f, 3.0f, -10.0f));
        Shader.SetGlobalVector("_SoundSources1", new Vector3(3.0f, 3.0f, -10.0f));
        Shader.SetGlobalVector("_SoundSources2", new Vector3(3.0f, 3.0f, -10.0f));
        Shader.SetGlobalVector ("_Wave0", new Vector4 (1.0f, 1.0f, 1.0f, 0.0f));
		Shader.SetGlobalVector ("_Wave1", new Vector4 (0.0f, 1.0f, 0.0f, 0.0f));
		Shader.SetGlobalVector ("_Wave2", new Vector4 (1.00f, 1.0f, 1.0f, 0.0f));

       // Shader.SetGlobalFloat("_T1", 0.0f);
      //  Shader.SetGlobalFloat("_T2", 0.0f);

        time[0] = 0.0f;
        time[1] = 0.0f;
        time[2] = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButtonDown("Fire1"))
        {
            time[count] = 0;
            Shader.SetGlobalVector("_SoundSources" + count, transform.position);
            count = (count + 1) % 3;
        }

        for (int i = 0; i < 3; i++)
        {
            time[i] += Time.deltaTime;
            Shader.SetGlobalVector ("_Colors"+i, new Vector4(0.0f, 1.0f, 0.0f, time[i]));
        }

        

        
	}
	
	void OnPostRender() {

	}


}
