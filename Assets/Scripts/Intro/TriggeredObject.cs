using UnityEngine;
using UnityEngine.UI;
using System.Collections;


abstract public class TriggeredObject : MonoBehaviour {
	public bool triggered = false;
	abstract public void Trigger(Collider mummyCollider);
}
