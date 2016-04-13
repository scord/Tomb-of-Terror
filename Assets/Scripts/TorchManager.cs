using UnityEngine;
using System.Collections;

public class TorchManager {

  private Light m_Light;
  private AudioSource m_AudioSource;
  private ParticleSystem[] m_ParticleSystems;

  private bool isActive;
  private float activeIntensity = 1.0f;
	// Use this for initialization

  public TorchManager(GameObject go) {
    m_ParticleSystems = go.GetComponentsInChildren<ParticleSystem>();
    m_AudioSource = go.GetComponentsInChildren<AudioSource>()[0];
    m_Light = go.GetComponentsInChildren<Light>()[0];
  }

  void SetPlayStop() {
    if (isActive) {
      foreach (ParticleSystem ps in m_ParticleSystems) {
        //ps.Play();
        ps.enableEmission = true;
      } 
      m_AudioSource.Play();
    } else {
      foreach (ParticleSystem ps in m_ParticleSystems) {
        //ps.Stop();
        ps.enableEmission = false;
      }
      m_AudioSource.Stop();
    }
  }

  public void SetLight() {
    float intensity = m_Light.GetComponent<FireLight>().intensityMultiplier;
    if ( isActive ) {
      m_Light.GetComponent<FireLight>().intensityMultiplier = Mathf.Lerp(intensity, activeIntensity, 0.01f);
    } else {
      m_Light.GetComponent<FireLight>().intensityMultiplier = Mathf.Lerp(intensity, 0.0f, 0.05f);
    }
  }

  public void Trigger() {
    isActive = !isActive;
    SetPlayStop();
  }

  public void Trigger(bool newValue) {
    isActive = newValue;
    SetPlayStop();
  }

  public void SetState(bool newValue) {
    isActive = newValue;
  }

  public bool GetState() {
    return isActive;
  }
}
