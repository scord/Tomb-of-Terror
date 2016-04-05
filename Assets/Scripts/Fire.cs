using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Fire : TargetInteract{
	[SyncVar (hook = "SyncActiveState")] private bool active = true;
	private ParticleSystem m_ParticleSystem;
  private AudioSource m_AudioSource;
  private Light m_Light;

	private void Start() {
		m_ParticleSystem = gameObject.GetComponent<ParticleSystem>();
    m_AudioSource = gameObject.GetComponentInChildren<AudioSource>();
    m_Light = gameObject.GetComponentsInChildren<Light>(true)[0];
		UpdateParticleSystem();
	}

	public override string GetText(){
		if(active )
			return "extinguish fire";
		return "fire it up";
	}

	public override void Trigger(){
    if (hasAuthority) {
      active = !active;
      UpdateParticleSystem();
    }
    else 
		  CmdSetActive(!active);
	}

  [Command]
  private void CmdSetActive(bool newActive) {
  	active = newActive;
  }

  [Client]
  private void SyncActiveState(bool newActive) {
  	active = newActive;
  	UpdateParticleSystem();
  }

  private void UpdateParticleSystem() {
  	if (active) {
  		m_ParticleSystem.Play();
      m_AudioSource.Play();
      m_Light.enabled = true;
  	} else {
  		m_ParticleSystem.Stop();
      m_AudioSource.Stop();
      m_Light.enabled = false;
  	}
  }
}


