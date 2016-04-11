using UnityEngine;
using System.Collections;

public class ServerCamera : MonoBehaviour {

  [SerializeField] private Camera m_Camera;

  void OnTriggerEnter(Collider co){
    Debug.LogWarning("Enter ma collider");
  }

  void OnTriggerExit(Collider co) {
    Debug.LogWarning("Exit ma collider");
  }

  void OnTriggerStay(Collider co) {
    Debug.LogWarning("Stay in ma collider");
  }
}
