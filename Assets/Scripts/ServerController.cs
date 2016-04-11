using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ServerController : MonoBehaviour {

  // Use this for initialization


  private int numberOfPlayers;
  private int lastNumberOfPlayers;

  [SerializeField] private GameObject cameraSpawns;
  [SerializeField] private GameObject cameraPrefab;
  private List<Camera> explorerCameras;
  private List<Camera> mummyCameras;

  private Camera m_MummyActive;
  private Camera m_ExplorerActive;

  private readonly Rect explorerCoord = new Rect(0, 0, 1, 0.5f); 
  private readonly Rect mummyCoord = new Rect(0, 0.5f, 1, 0.5f);
  private Camera[] cameras;
  void Start () {
    lastNumberOfPlayers = NetworkServer.connections.Count;
    //cameraSpawns = (GameObject)Instantiate(chosenPlayer, spawnTransform.position, spawnTransform.rotation);
    GameObject go = (GameObject)Instantiate(cameraSpawns, Vector3.zero, Quaternion.identity);
    SpawnCameras("ServerExplorerCameraSpawn", go, ref explorerCameras);
    SpawnCameras("ServerMummyCameraSpawn", go, ref mummyCameras, true);

    //Debug.Log(explorerCameras[0]);
    explorerCameras[0].enabled = true;
    mummyCameras[0].enabled = true;
    m_ExplorerActive = explorerCameras[0];
    m_MummyActive = mummyCameras[0];
    Debug.Log(explorerCameras.Count);
  }

  void SpawnCameras(string tag, GameObject source, ref List<Camera> dest, bool isMummy = false) {

    dest = dest ?? new List<Camera>();

    GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);

    foreach(GameObject go in gos) {
      GameObject g = (GameObject)Instantiate(cameraPrefab, go.transform.position, go.transform.rotation);
      g.transform.parent = go.transform;
      Camera c_go = g.GetComponent<Camera>();
      if ( isMummy ) {
        c_go.rect = mummyCoord;
      } else {
        c_go.rect = explorerCoord;
      }
      dest.Add(c_go);
    }

  }

  void Update() {
    if (Input.GetKeyDown("1")) {
      m_MummyActive.GetComponent<MouseLook>().enabled = false;
      m_ExplorerActive.GetComponent<MouseLook>().enabled = !m_ExplorerActive.GetComponent<MouseLook>().enabled;
    } else if (Input.GetKeyDown("2")) {
      m_ExplorerActive.GetComponent<MouseLook>().enabled = false;      
      m_MummyActive.GetComponent<MouseLook>().enabled = !m_MummyActive.GetComponent<MouseLook>().enabled;
    }
    /*numberOfPlayers = NetworkServer.connections.Count;
    if ( lastNumberOfPlayers != numberOfPlayers ) {
      GameObject[] goa= GameObject.FindGameObjectsWithTag("Player");
      if (goa.Length != 0) {
        GameObject go = goa[0];
        Camera[] c = go.GetComponentsInChildren<Camera>();
        if (c.Length != 0) {
          c[0].enabled = true;
        }
      }
    }*/
  }

}