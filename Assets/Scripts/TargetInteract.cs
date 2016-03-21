using UnityEngine;
using UnityEngine.UI;
using System.Collections;

abstract public class TargetInteract : MonoBehaviour {

	abstract public string GetText();
	abstract public void Trigger();

}