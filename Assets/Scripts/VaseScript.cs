using UnityEngine;
using System.Collections;

public class VaseScript : MonoBehaviour {
	void OnCollisionEnter(Collision collision) {
        Debug.Log(collision.collider);
    }
}
