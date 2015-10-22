using UnityEngine;
using System.Collections;

public class SoundVision : MonoBehaviour {
	
	public Shader shader;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
		gameObject.GetComponent<Camera>().SetReplacementShader(shader, "");
		//material.SetFloat ("speed", 50.0f);
		
		Shader.SetGlobalColor ("_BaseColor", new Color (0.8f, 0.4f, 1.0f, 1.0f));
		Shader.SetGlobalVector ("_SoundSource", new Vector3 (6.0f, 3.0f, -10.0f));
		Shader.SetGlobalVector ("_SoundSources0", new Vector3 (3.0f, 3.0f, -10.0f));
		Shader.SetGlobalVector ("_Colors0", new Vector3 (1.0f, 0.5f, 0.0f));
		Shader.SetGlobalVector ("_Colors1", new Vector3 (0.5f, 1.0f, 0.5f));
		Shader.SetGlobalVector ("_Colors2", new Vector3 (0.25f, 0.5f, 1.0f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnPostRender() {

	}

	void OnRenderImage (RenderTexture source, RenderTexture destination){

	}
}
