using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class SoundVision : MonoBehaviour
{

    public Shader shader;
	public HeartRateManager heartRateManager;
    private int maxWaves = 64; //number of possible simultaneous waves

    bool echoLocation = false;
    private float echoTime = 0;
	float maxVolume = 50;
    public int maxLength = 32;
    public int numfree = 0;
    public Color color = new Color(0, 0.85f, 1, 1);
    float timer = 0;
    Waves waves;
    public Texture2D wavetex;
    // Use this for initialization
    List<WaveSource> waveSources;

    void Start()
    {
        EchoLocate();
        GetComponent<Camera>().SetReplacementShader(shader, "");
        waves = new Waves(maxWaves, maxLength);
        waveSources = new List<WaveSource>();
        UpdateAudioSources();
   
    }

    // Update is called once per frame

    WaveSource ContainsAudioSource(List<WaveSource> sources, AudioSource source, ref bool found)
    {
        foreach (WaveSource waveSource in sources)
        {
            if (waveSource.audioSource.GetInstanceID() == source.GetInstanceID())
            {
                found = true;
                return waveSource;
            }
        }
        found = false;
        return new WaveSource();
    }

    void GetAudioSources()
    {
        waveSources = new List<WaveSource>();

        foreach (AudioSource source in FindObjectsOfType<AudioSource>())
        {
            if (source.gameObject.layer != LayerMask.NameToLayer("Ignore Sound Vision"))
            {
                int i = waves.free.Pop();
                WaveSource waveSource = new WaveSource(source, i);
                waveSources.Add(waveSource);


                waveSource.SendToShader();

                if ( i == 0 ) break;
            }
        }
    }


    void UpdateAudioSources()
    {
    
        List<AudioSource> sortedList = (new List<AudioSource>(FindObjectsOfType<AudioSource>())).OrderBy(o => Vector3.Distance(o.transform.position, transform.position)).ToList();
        foreach (AudioSource source in sortedList)
        {
            if (source.gameObject.layer == LayerMask.NameToLayer("Ignore Sound Vision"))
                sortedList.Remove(source);
        }
        int sourceCount = 0;
        foreach (AudioSource source in sortedList)
        {
            bool found = false;
            WaveSource foundWaveSource = ContainsAudioSource(waveSources, source, ref found);

            if (sourceCount < Mathf.Min(sortedList.Count, 32))
            {

                if (!found)
                {
                    if (waves.free.Count == 0) break;
                    int i = waves.free.Pop();

                    WaveSource waveSource = new WaveSource(source, i);
                    waveSources.Add(waveSource);


                    waveSource.SendToShader();

                    
                }
            }
            else if (found)
            {
                waves.release(foundWaveSource);
                waveSources.Remove(foundWaveSource);
            }

            sourceCount++;
        }
        

    }

    

    public void EchoLocate()
    {
        echoLocation = true;
        Shader.SetGlobalColor("_EchoColor", color);
        Shader.SetGlobalVector("_EchoSource", transform.position);
        Shader.SetGlobalFloat("_EchoTime", 0);
    }
    
    void Update()
    {
        
		float dtime = Time.deltaTime;
        timer += dtime;

        if (timer > 5)
        {
            UpdateAudioSources();
            timer = 0;
        }

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

        for (int i = 0; i < waveSources.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, waveSources[i].audioSource.transform.position);
            Color c;

            c = waveSources[i].GetCurrentColor(color);

            waveSources[i] = waves.getFreeWave(waveSources[i]);
            waves.colors[waveSources[i].index] = waves.AddColor(waves.colors[waveSources[i].index], c);
        }

     


        waves.UpdateTexture(waveSources);
        wavetex = waves.texture;
        waves.SendToShader();

        waves.release();
        Debug.Log(waves.free.Count);
    }

    

    struct Waves
    {
        public Texture2D texture;
        public Color[][] colors;
        public Stack<int> free;
        public int numWaves;
        int waveLength;
        List<int> enabled;

        public Waves(int numWaves, int waveLength)
        {
            this.numWaves = numWaves;
            this.waveLength = waveLength;

            free = new Stack<int>();

            for (int i = 0; i < numWaves; i++)
            {
                free.Push(i);
            }

            colors = new Color[numWaves][];
            enabled = new List<int>();
            texture = new Texture2D(waveLength, numWaves);
     
            texture.wrapMode = (TextureWrapMode)WrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            for (int i = 0; i < numWaves; i++)
            {
                colors[i] = new Color[waveLength];
                for (int j = 0; j < waveLength; j++)
                {
                    colors[i][j] = (Color.black);
                }
                enabled.Add(0);
            }

            Shader.SetGlobalFloat("_N", numWaves);
        }

        public WaveSource getFreeWave(WaveSource source)
        {
            if (free.Contains(source.index)) { }
            else if (source.GetDeltaMovement() > 1.5f && free.Count > 0) source.deltaMovement = 0;
            else return source;

            source.index = free.Pop();
            enabled[source.index] = 1;
            source.SendToShader();

            return source;
        }

        public void UpdateTexture(List<WaveSource> sources)
        {
            List<int> currentWaves = new List<int>();
            foreach (WaveSource source in sources)
            {
                currentWaves.Add(source.index);
            }
            for (int i = 0; i < numWaves; i++)
            {
                if (!currentWaves.Contains(i))
                    colors[i] = AddColor(colors[i], new Color(0, 0, 0, 1));

                texture.SetPixels(0, i, texture.width, 1, colors[i]);
            }
            texture.Apply(true);
 
        }

        public void SendToShader()
        {
            for (int i = 0; i < numWaves; i++)
                Shader.SetGlobalInt("_Enabled"+i, enabled[i]);
            Shader.SetGlobalTexture("_Waves", texture);
        }

        public void release(WaveSource waveSource)
        {
            int i = waveSource.index;
            free.Push(i);
            enabled[i] = 0;
        }
        public void release()
        {
            for (int i = 0; i < numWaves; i++)
            {
                bool empty = true;
                for (int j = 0; j < waveLength; j++)
                {
                    if (colors[i][j] != Color.black)
                    {
                        empty = false;
                        break;
                    }
                }

                if (empty && !free.Contains(i))
                {
                    free.Push(i);
                    enabled[i] = 0;
                }
            }
        }

        public Color[] AddColor(Color[] wave, Color color)
        {
            for (int i = wave.Length - 1; i > 0; i--)
            {
                wave[i] = wave[i - 1];
            }
            wave[0] = color;

            return wave;
        }
    }

    struct WaveSource
    {
        public AudioSource audioSource;
        Vector3 prevPosition;
        public float deltaMovement;
        public int index;
        float volume;

        public WaveSource(AudioSource audioSource, int i)
        {
            this.audioSource = audioSource;
            index = i;
            deltaMovement = 0.1f;
            prevPosition = audioSource.transform.position;
            volume = 1f;
        }

        public void SendToShader()
        {
            Shader.SetGlobalVector("_SoundSource" + index, audioSource.transform.position);
            Shader.SetGlobalVector("_Volume" + index, new Vector2(volume, 0f));
        }

        public Color GetCurrentColor(Color baseColor)
        {
            float[] spectrum = new float[64];

            audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
            float summedFreq = 0;
            float level = 0;
            for (int i = 0; i < spectrum.Length; i++)
            {
                summedFreq += spectrum[i] * i;
                level += spectrum[i];

            }

            float averageFreq = ((summedFreq / level)) / (spectrum.Length - 4);
         
            if (level < 0)
                level = 0;

            if (level < 0.0000001)
                level = 0;
            level = Mathf.Pow(level * 100,0.5f)/5f;

            


            //return new Color(level * averageFreq * 4, level * (1 - averageFreq * 4), level, 1);

            return baseColor *(new Color(level * averageFreq * 4, level * (1 - averageFreq * 4), level, 1));
        }
        public float GetDeltaMovement()
        {
            
            deltaMovement += Vector3.Distance(audioSource.transform.position, prevPosition);
            prevPosition = audioSource.transform.position;
            return deltaMovement;
        }

    }

}
