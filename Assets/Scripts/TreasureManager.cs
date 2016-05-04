using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class TreasureManager : MonoBehaviour {

  private bool alive = true;
  [SerializeField] private int air = 50;
  private float interval = 1 ;
  private float airTimer = 0;

  private NetworkManagerCustom m_NetworkManager;
  private List<PlayerController> m_PlayerControllerList;

  public delegate void TreasureDelegate(PlayerController pc, int value);
  public event TreasureDelegate UpdateTreasurePoints;

  void Awake() {
    m_NetworkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManagerCustom>();
    m_PlayerControllerList = new List<PlayerController>();
  }
  void Start(){
    m_NetworkManager.AddedPlayerCallback += UpdateListPlayers;
    alive = true;
    interval = 1;
    airTimer = 0;
  }

  void OnDisable() {
    if ( m_NetworkManager == null) return;
    m_NetworkManager.AddedPlayerCallback -= UpdateListPlayers;
  }

  public void UpdateListPlayers() {
    m_PlayerControllerList.Clear();
    PlayerController[] pcs = FindObjectsOfType<PlayerController>();
    foreach(PlayerController pc in pcs) {
      m_PlayerControllerList.Add(pc);
    }
  }

  public void UpdateAir(bool fire){
    if(!fire)
      interval += 0.5f;
    else
      interval -= 0.5f;
  }

  void Update(){
    UpdateOxigen();
    if ( UpdateTreasurePoints != null ) {
      UpdateTreasurePoints(m_PlayerControllerList[0], 50);
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
