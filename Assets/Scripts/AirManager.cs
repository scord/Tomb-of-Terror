using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AirManager : MonoBehaviour {

  private bool alive = true;
  [SerializeField] private int air = 50;
  private float interval = 1 ;
  private float airTimer = 0;

  private NetworkManagerCustom m_NetworkManager;

  public delegate void AirDelegate(int newAir);
  public event AirDelegate EventAirUpdate;

  void Awake() {
    m_NetworkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManagerCustom>();
  }
  void Start(){
    alive = true;
    interval = 1;
    airTimer = 0;
  }

  public void UpdateAir(bool fire){
    if(!fire)
      interval += 0.5f;
    else
      interval -= 0.5f;
  }

  void Update(){
    UpdateOxigen();
    if ( EventAirUpdate != null ) {
      EventAirUpdate(air);
    }
    CheckDead();
  }

  void CheckDead() {
    if ( air <= 0 ) {
      m_NetworkManager.EndGame();
    }
  }

  private void UpdateOxigen(){
    if( alive ){
      if( interval > airTimer ){
        airTimer += Time.deltaTime;
      }
      else{
        air--;
        airTimer = 0;
        //Debug.Log(air);
      }
    }
  }
}
