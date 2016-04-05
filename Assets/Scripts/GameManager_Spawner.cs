using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameManager_Spawner : NetworkBehaviour {

  [SerializeField] private GameObject objectPrefab;
  [SerializeField] private string m_Tag;
  private GameObject[] objectSpawns;

  public override void OnStartServer() {
    objectSpawns = GameObject.FindGameObjectsWithTag(m_Tag);
    foreach(GameObject go in objectSpawns) {
      SpawnObject(go);
    }
  }

  void SpawnObject(GameObject goLocation) {
    GameObject go = GameObject.Instantiate(objectPrefab, goLocation.transform.position, goLocation.transform.rotation) as GameObject;
    go.transform.parent = goLocation.transform;
    NetworkServer.Spawn(go);
    Debug.Log("Object Spawned" + go.transform.parent.gameObject);
  }

}
