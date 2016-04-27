using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjTrigger : MonoBehaviour {

  [SerializeField]
	private TriggeredObject targetObject;

  void OnTriggerEnter(Collider other){
    Debug.Log(targetObject.triggered);
      targetObject.Trigger(other);
    if(targetObject.triggered)
      Destroy(gameObject);
  }
  void OnTriggerStay(Collider other){
    Debug.Log(targetObject.triggered);
      targetObject.Trigger(other);
    if(targetObject.triggered)
      Destroy(gameObject);
  }
}
