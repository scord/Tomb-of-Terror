using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class LevelTunnel : MonoBehaviour {
  private LevelRoom room1;
  private LevelRoom room2;

  
  public void Initialize(LevelRoom first, LevelRoom second){
    this.room1 = first;
    this.room2 = second;
  }

  public LevelRoom Room1(){
      return this.room1;
  }
  public LevelRoom Room2(){
      return this.room2;
  }
}
