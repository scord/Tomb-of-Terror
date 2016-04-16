using UnityEngine;
using UnityEngine.UI;
using System.Collections;


abstract public class TriggeredObject : MonoBehaviour {
	abstract public void Trigger(Collider mummyCollider);
}
