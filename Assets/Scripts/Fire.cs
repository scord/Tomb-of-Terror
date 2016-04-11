using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Fire : TargetInteract{
	[SyncVar (hook = "SyncActiveState")] private bool active = true;
    [SerializeField]
    private ParticleSystem[] m_ParticleSystems;
    private AudioSource m_AudioSource;
  private Light m_Light;

	private void Start() {
        m_ParticleSystems = GetComponentsInChildren<ParticleSystem>();
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


    void Update()
    {
        SetLight();
    }

    void SetLight()
    {
        float intensity = m_Light.GetComponent<FireLight>().intensityMultiplier;
        if (active)
        {
            m_Light.GetComponent<FireLight>().intensityMultiplier = Mathf.Lerp(intensity, 1.0f, 0.01f);
        }
        else
        {
            m_Light.GetComponent<FireLight>().intensityMultiplier = Mathf.Lerp(intensity, 0.0f, 0.05f);
        }
    }

    private void UpdateParticleSystem() {
  	if (active) {
      foreach (ParticleSystem m_ParticleSystem in m_ParticleSystems)
      {
        m_ParticleSystem.Play();
      }
      m_AudioSource.Play();
      m_Light.enabled = true;
  	} else {
      foreach (ParticleSystem m_ParticleSystem in m_ParticleSystems)
      {
        m_ParticleSystem.Stop();
      }
      m_AudioSource.Stop();
      m_Light.enabled = false;
  	}
  }
}


