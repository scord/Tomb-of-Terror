using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjTrigger : MonoBehaviour {

  [SerializeField]
	private TriggeredObject targetObject;

  void OnTriggerEnter(Collider other){
    targetObject.Trigger(other);
    Destroy(gameObject);
  }


}
