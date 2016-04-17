using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

abstract public class TargetInteract : NetworkBehaviour {

	abstract public string GetText();
	abstract public void Trigger();

}
