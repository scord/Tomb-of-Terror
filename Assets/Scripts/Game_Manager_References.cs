using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Game_Manager_References : MonoBehaviour {

  private List<InteractScript> interactObjects;
  private List<GameObject> pickableObjects;
  private List<GameObject> smallPrizes;
	// Use this for initialization
	void Start () {
    GameObject[] tmp = GameObject.FindGameObjectsWithTag("Interaction");
    interactObjects = new List<InteractScript>();

    foreach( GameObject go in tmp) {
      interactObjects.Add(go.GetComponent<InteractScript>());
    }

    pickableObjects = new List<GameObject>();
    tmp = GameObject.FindGameObjectsWithTag("PickUp");
    foreach( GameObject go in tmp) {
      pickableObjects.Add(go);
    }
    tmp = GameObject.FindGameObjectsWithTag("Prize");
    foreach( GameObject go in tmp) {
      pickableObjects.Add(go);
    }

    smallPrizes = new List<GameObject>();
    tmp = GameObject.FindGameObjectsWithTag("SmallPrize");
    foreach( GameObject go in tmp) {
      pickableObjects.Add(go);
    }
    // Debug.Log("Found: " + pickableObjects.Length);
	}

	// Update is called once per frame
	void Update () {

	}

  public int GetInteractObjectIndex(InteractScript o) {
    return interactObjects.IndexOf(o);
  }

  public InteractScript GetInteractObject(int index) {
    return interactObjects[index];
  }

  public void SetInteractObject(InteractScript o, int index) {
    if ( index != -1 && index < interactObjects.Count) {
      interactObjects[index] = o;
    }
  }

  public void RemovePickableAtIndex(GameObject o, int index) {
    pickableObjects.RemoveAt(index);
  }

  public int GetPickUpObjectIndex(GameObject go) {
    return pickableObjects.IndexOf(go);
  }

  public GameObject GetPickUpObject(int index) {
    return pickableObjects[index];
  }
  public void SetPickUpObject(GameObject go, int index) {
    if ( index != -1 && index < pickableObjects.Count) {
      pickableObjects[index] = go;
    }
  }
}
