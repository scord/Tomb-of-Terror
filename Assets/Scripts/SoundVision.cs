using UnityEngine;
using System.Collections;
using System;

public class SoundVision : MonoBehaviour
{

    public Shader shader;
    public AudioSource audioSource;
	public HeartRateManager heartRateManager;
    int maxWaves = 64; //number of possible simultaneous waves

    bool echoLocation = false;
    private float echoTime = 0;

	float maxVolume = 50;
    public int maxLength = 64;

    public ArrayList sources;

    public float timer = 0;
    public Texture2D waves;
    public Stack freeWaves;
    public int stacksize = 0;
    ArrayList prevPositions;
    ArrayList volume;
    ArrayList counts;
    Color[][] wave;
    // Use this for initialization

    void AddSource(AudioSource source)
    {
        sources.Add(source);
        counts.Add(sources.Count - 1);
        volume.Add(1f);

    }
    void Start()
    {
        gameObject.GetComponent<Camera>().SetReplacementShader(shader, "");
        freeWaves = new Stack();

        sources = new ArrayList();
        sources.AddRange(FindObjectsOfType<AudioSource>());
        Debug.Log(sources.Count);
        wave = new Color[maxWaves][];
     
        prevPositions = new ArrayList();
        counts = new ArrayList();
        volume = new ArrayList();
        for (int i = 0; i < maxWaves; i++)
        {
            freeWaves.Push(i);
        }
        for (int i = 0; i < sources.Count; i++)
        {
            prevPositions.Add(((AudioSource)sources[i]).transform.position);
            int j = (int)freeWaves.Pop();
 
            counts.Add(j);
            
            Shader.SetGlobalVector("_SoundSource" + j, ((AudioSource)sources[i]).transform.position);
            volume.Add(1f);
            Shader.SetGlobalVector("_Volume" + j, new Vector2((float)volume[i],0f));
        }


        waves = new Texture2D(maxLength, maxWaves);
        waves.wrapMode = (TextureWrapMode)WrapMode.Clamp;
        waves.filterMode = FilterMode.Point;
        for (int i = 0; i < maxWaves; i++)
        {
            wave[i] = new Color[maxLength];
            for (int j = 0; j < maxLength; j++)
            {
                wave[i][j] = (Color.black);
            }
        }

        

        Shader.SetGlobalFloat("_N", maxWaves);
        Shader.SetGlobalInt("_CurrentWave", 0);
    }

    // Update is called once per frame

    Texture2D updateTexture(Texture2D tex, int rowIndex, Color[] data)
    {
   
        for (int i = 0; i < tex.width; i++)
        {
            tex.SetPixel(i, rowIndex, (Color)data[i]);
        }

        return tex;
    }

    void releaseWaves()
    {

        for (int i = 0; i < maxWaves; i++)
        {
            bool empty = true;
            for (int j = 0; j < maxLength; j++)
            {
                if ((Color)wave[i][j] != Color.black)
                {
                    empty = false;
                    break;
                }
            }

            if (empty && !freeWaves.Contains(i))
            {
                freeWaves.Push(i);
            }
        }
        
    }

    Color[] AddColor(Color[] wave, Color color)
    {
        for (int i = wave.Length-1; i > 0; i--)
        {
            wave[i] = wave[i - 1];
        }
        wave[0] = color;

        return wave;
    }

    public void EchoLocate()
    {
        echoLocation = true;
        Shader.SetGlobalVector("_EchoSource", transform.position);
        Shader.SetGlobalFloat("_EchoTime", 0);
    }
    
    void Update()
    {
		float dtime = Time.deltaTime;


        timer += dtime;

        if (echoLocation)
        {
            if (echoTime > 5)
            {
                echoLocation = false;
                echoTime = 0;
            }
            echoTime += dtime;
            Shader.SetGlobalFloat("_EchoTime", echoTime);
        }


        if (timer > 1/75.0f)
        {
            
            timer = 0;
            for (int x = 0; x < sources.Count; x++)
            {
                float[] spectrum = new float[64];

                ((AudioSource)sources[x]).GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
                float summedFreq = 0;
                float level = 0;
                for (int i = 0; i < spectrum.Length; i++)
                {
                    summedFreq += spectrum[i] * i;
                    level += spectrum[i];
 
                }

                float averageFreq = ((summedFreq / level))/(spectrum.Length - 4);
                if (averageFreq < 0.011)
                    level = level - (0.011f - averageFreq)*100;
                if (averageFreq > 0.6)
                    level = level - (averageFreq - 0.6f) * 100;
                if (level < 0)
                    level = 0;
                level = level*20;

                Color c = new Color(level*averageFreq*4, level*(1-averageFreq*4), level, 1);
            
                
                if (level > 0 && freeWaves.Contains(counts[x]))
                    counts[x] = freeWaves.Pop();

                wave[(int)counts[x]] = AddColor(wave[(int)counts[x]], (c + (Color)wave[(int)counts[x]][0]) / 2);
                Shader.SetGlobalVector("_SoundSource" + counts[x], ((AudioSource)sources[x]).transform.position);
                Shader.SetGlobalVector("_Volume" + (int)counts[x], new Vector2((float)volume[x], 0));
            }
 
            for (int i = 0; i < maxWaves; i++)
            {
                if (!counts.Contains(i))
                {
                    wave[i] = AddColor(wave[i], new Color(0, 0, 0, 1));
        
                }

                waves = updateTexture(waves, i, wave[i]);       
            }
            waves.Apply(true);

            
            
            Shader.SetGlobalTexture("_Waves", waves);

            for (int x = 0; x < sources.Count; x++)
            {
                
                if (Vector3.Magnitude((Vector3)prevPositions[x] - ((AudioSource)sources[x]).transform.position) > 0.1)
                {
                    if (freeWaves.Count > 0)
                        counts[x] = (int)freeWaves.Pop();

                    Shader.SetGlobalVector("_SoundSource" + counts[x], ((AudioSource)sources[x]).transform.position);
                    Shader.SetGlobalVector("_Volume" + (int)counts[x], new Vector2((float)volume[x],0));
                }

                prevPositions[x] = ((AudioSource)sources[x]).transform.position;
            }
            releaseWaves();
            stacksize = freeWaves.Count;
        }
    }

    void OnPostRender()
    {

    }

}
