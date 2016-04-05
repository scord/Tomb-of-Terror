using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Game_Manager_References : MonoBehaviour {

  private InteractScript[] interactObjects;
  private GameObject[] pickableObjects;
	// Use this for initialization
	void Start () {
    GameObject[] tmp = GameObject.FindGameObjectsWithTag("Interaction");
    interactObjects = new InteractScript[tmp.Length];

    for(int i = 0; i < tmp.Length; i++) {
      interactObjects[i] = tmp[i].GetComponent<InteractScript>();
    }

    pickableObjects = GameObject.FindGameObjectsWithTag("PickUp");
    Debug.Log("Found: " + pickableObjects.Length);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public int GetInteractObjectIndex(InteractScript o) {
    return System.Array.IndexOf(interactObjects, o);
  }

  public InteractScript GetInteractObject(int index) {
    return interactObjects[index];
  }

  public void SetInteractObject(InteractScript o, int index) {
    if ( index != -1 && index < interactObjects.Length) {
      interactObjects[index] = o;
    }
  }

  public int GetPickUpObjectIndex(GameObject go) {
    return System.Array.IndexOf(pickableObjects, go);
  }

  public GameObject GetPickUpObject(int index) {
    return pickableObjects[index];
  }
  public void SetPickUpObject(GameObject go, int index) {
    if ( index != -1 && index < pickableObjects.Length) {
      pickableObjects[index] = go;
    }
  }
}
