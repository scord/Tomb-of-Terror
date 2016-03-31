using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ExplorerTorch : NetworkBehaviour {
  [SerializeField] private Light m_Light;
  [SerializeField] private AudioSource m_AudioSource;
  [SerializeField] private ParticleSystem m_ParticleSystem;

  [SyncVar (hook = "UpdateTorchStatus")] private bool isActive;

  private float activeIntensity = 2.3f;

  void Start() {
    if ( isLocalPlayer ) {
      CmdChangeActive(true);
    }
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
      m_ParticleSystem.Play();
      m_AudioSource.Play();
    } else {
      m_ParticleSystem.Stop();
      m_AudioSource.Stop();
    }
  }

  void SetLight() {
    if ( isActive ) {
      m_Light.intensity = Mathf.Lerp(m_Light.intensity, activeIntensity, 0.01f);
    } else {
      m_Light.intensity = Mathf.Lerp(m_Light.intensity, 0, 0.01f);
    }
  }
}
