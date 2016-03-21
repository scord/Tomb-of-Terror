using UnityEngine;
using System.Collections;

public class TombManager : MonoBehaviour {

	public Tomb tombPrefab;
	private Tomb tombInstance;

	private void Start () {
        BeginGame();
	}
	
	private void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			RestartGame();
		}
	}


	public void BeginGame () {
		tombInstance = Instantiate(tombPrefab) as Tomb;
		tombInstance.Generate();
	}

	private void RestartGame () {
		StopAllCoroutines();
		Destroy(tombInstance.gameObject);
		BeginGame();
	}
}
