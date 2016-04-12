using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AirManager : MonoBehaviour {

  private bool alive = true;
  [SerializeField] private int air = 50;
  private float interval = 1 ;
  private float airTimer = 0;

  void Start(){
    alive = true;
    interval = 1;
    airTimer = 0;
  }

  public void UpdateAir(bool fire){
    Debug.Log(fire);
    if(!fire)
      interval += 0.5f;
    else
      interval -= 0.5f;
  }

  void Update(){
    UpdateOxigen();
  }

  private void UpdateOxigen(){
    if( alive ){
      if( interval > airTimer ){
        airTimer += Time.deltaTime;
      }
      else{
        air--;
        airTimer = 0;
        Debug.Log(air);
      }

      if(air < 0) {
        alive = false;
        Debug.Log("YOU DIEDED");
        SceneManager.LoadScene ("Scenes/endgame");
      }
    }
  }
}
