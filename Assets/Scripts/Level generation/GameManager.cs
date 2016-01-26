using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Maze mazePrefab;
	public Player mummySpawn;
	public Player adventurerSpawn;

	private Maze mazeInstance;
	private Creator god;



	private void Start () {
		BeginGame();
	}
	
	private void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			RestartGame();
		}
	}

	private void BeginGame () {
		mazeInstance = Instantiate(mazePrefab) as Maze;
		mazeInstance.Generate();

		// adventurer = Instantiate(adventurerPrefab) as Player;
		adventurerSpawn.SetLocation(mazeInstance.GetCell(new IntVector2(mazeInstance.size.x-1, mazeInstance.size.z-1), true));

		// mummy = Instantiate(mummyPrefab) as Player;
		mummySpawn.SetLocation(mazeInstance.GetCell(new IntVector2(0, 0), true));
	}

	private void RestartGame () {
		StopAllCoroutines();
		Destroy(mazeInstance.gameObject);

		BeginGame();
	}
}
