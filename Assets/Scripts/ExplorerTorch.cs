using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ExplorerTorch : NetworkBehaviour {
  [SerializeField] private Light m_Light;
  [SerializeField] private AudioSource m_AudioSource;
  [SerializeField] private ParticleSystem[] m_ParticleSystems;

  [SyncVar (hook = "UpdateTorchStatus")] private bool isActive;

  private float activeIntensity = 1.0f;

  void Start() {
    if ( isLocalPlayer ) {
      CmdChangeActive(true);
    }

	m_ParticleSystems = GetComponentsInChildren<ParticleSystem> ();
    SetPlayStop();
  }

  void Update() {
    SetLight();
    if ( !isLocalPlayer ) {
      return;
    }
    if ( isLocalPlayer ) {
      if (Input.GetButtonDown("Fire1")) {
        CmdChangeActive(!isActive);
      }
    } 
  }

  [Command]
  void CmdChangeActive(bool newValue) {
    isActive = newValue;
  }

  [Client]
  void UpdateTorchStatus(bool newValue) {
    isActive = newValue;
    SetPlayStop();
  }

  void SetPlayStop() {
    if (isActive) {
	  foreach (ParticleSystem m_ParticleSystem in m_ParticleSystems)
	  {
	  	m_ParticleSystem.Play();
	  } 
      m_AudioSource.Play();
    } else {
	  foreach (ParticleSystem m_ParticleSystem in m_ParticleSystems)
	  {
		m_ParticleSystem.Stop();
	  } 
      m_AudioSource.Stop();
    }
  }

  void SetLight() {
    float intensity = m_Light.GetComponent<FireLight>().intensityMultiplier;
    if ( isActive ) {
      m_Light.GetComponent<FireLight>().intensityMultiplier = Mathf.Lerp(intensity, activeIntensity, 0.01f);
    } else {
      m_Light.GetComponent<FireLight>().intensityMultiplier = Mathf.Lerp(intensity, 0.0f, 0.05f);
    }
  }
}
