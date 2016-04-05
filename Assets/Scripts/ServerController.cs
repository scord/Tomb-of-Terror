using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerController : MonoBehaviour {

  // Use this for initialization


  private int numberOfPlayers;
  private int lastNumberOfPlayers;

  private Camera[] cameras;
  void Start () {
    lastNumberOfPlayers = NetworkServer.connections.Count;
  }

  void Update() {
    numberOfPlayers = NetworkServer.connections.Count;
    if ( lastNumberOfPlayers != numberOfPlayers ) {
      GameObject[] goa= GameObject.FindGameObjectsWithTag("Player");
      if (goa.Length != 0) {
        GameObject go = goa[0];
        Camera[] c = go.GetComponentsInChildren<Camera>();
        if (c.Length != 0) {
          c[0].enabled = true;
        }
      }
    }
  }

}