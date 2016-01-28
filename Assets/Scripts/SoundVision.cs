using UnityEngine;
using System.Collections;

public class SoundVision : MonoBehaviour
{

    public Shader shader;
    public AudioSource audioSource;
	public HeartRateManager heartRateManager;
    public int n = 10; //number of possible simultaneous waves
    float[] time;
    int[] active;
    int count = 0;
	float maxVolume = 50;
	float timer = 0;
	public float beatInterval;

    // Use this for initialization
    void Start()
    {
        //gameObject.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        gameObject.GetComponent<Camera>().SetReplacementShader(shader, "");

        //GameObject[] objects = GameObject.FindGameObjectsWithTag ("AudioReflective");

        time = new float[n];
        active = new int[n];

        Shader.SetGlobalFloat("_N", n);
        Shader.SetGlobalInt("_CurrentWave", 0);

        //Shader.SetGlobalVector("_RimColor", new Vector4(0.0f, 0.8f, 1.0f, 1.0f));
		heartRateManager = GameObject.Find ("HeartRate").GetComponent<HeartRateManager> ();

        for (int i = 0; i < n; i++)
        {
            Shader.SetGlobalVector("_SoundSource" + i, new Vector3(0.0f, 0.0f, 0.0f));
            time[i] = 0.0f;
            active[i] = 0;
        }
    }

	public void CreateSound(Vector3 position, float volume)
	{
		// cap at maxVolume
		if (volume > maxVolume) {
			volume = maxVolume;
		}

		// normalise between 0 and 1
		volume = volume / maxVolume;

		Shader.SetGlobalInt("_CurrentWave", count);
		time[count] = 0;
		active[count] = 1;
		Shader.SetGlobalVector("_SoundSource" + count, position);
		Shader.SetGlobalVector ("_Volume" + count, new Vector2(volume, 0));
		count = (count + 1) % n;
		//audioSource.time = 0.5f;
		//audioSource.Play();
	}

    // Update is called once per frame
    void Update()
    {
		float dtime = Time.deltaTime;
		beatInterval = 60.0f / heartRateManager.HeartRate;

		timer += dtime;
		if (timer > beatInterval) {
			if (GameObject.FindGameObjectWithTag("ExplorerHeart"))
			{
				Vector3 explorerPosition = GameObject.FindGameObjectWithTag("ExplorerHeart").transform.position;
				CreateSound (explorerPosition, 15);
			}
			timer = 0;
		}


        if (Input.GetButtonDown("Fire1"))
        {
            Shader.SetGlobalInt("_CurrentWave", count);
            time[count] = 0;
            active[count] = 1;
            Shader.SetGlobalVector("_SoundSource" + count, transform.position);
            Shader.SetGlobalVector("_Volume" + count, new Vector2(1, 0));
            count = (count + 1) % n;
            audioSource.time = 0.5f;
            audioSource.Play();
        }

        for (int i = 0; i < n; i++)
        {
            if (active[i] == 1)
            {
                time[i] += Time.deltaTime;

            }
            Shader.SetGlobalVector("_Colors" + i, active[i] * (new Vector4(0.5f, 1.0f, 1.0f, time[i])));
        }
    }

    void OnPostRender()
    {

    }


}
