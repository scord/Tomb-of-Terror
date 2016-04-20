using UnityEngine;
using System.Collections;

public class DestroyAfterPickup : MonoBehaviour {


  [SerializeField] private GameObject m_DestroyTarget;

  public void OnPickedUp() {
    Destroy(m_DestroyTarget ?? gameObject);
  }
}
