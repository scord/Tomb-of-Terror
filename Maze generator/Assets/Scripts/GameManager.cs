﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Maze mazePrefab;
	public Player mummyPrefab;
	public Player adventurerPrefab;
	public Prize prizePrefab;



	private Player mummy;
	private Player adventurer;
	private Prize treasure; 
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
		Camera.main.clearFlags = CameraClearFlags.Skybox;
		Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
		mazeInstance = Instantiate(mazePrefab) as Maze;
		mazeInstance.Generate();

		adventurer = Instantiate(adventurerPrefab) as Player;
		adventurer.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));

		mummy = Instantiate(mummyPrefab) as Player;
		mummy.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));

		treasure = Instantiate(prizePrefab) as Prize;
		treasure.SetLocation(mummy.GetCell());
		
		Camera.main.clearFlags = CameraClearFlags.Depth;
		Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);
	}

	private void RestartGame () {
		StopAllCoroutines();
		Destroy(mazeInstance.gameObject);
		if (mummy != null) {
			Destroy(mummy.gameObject);
			Destroy(adventurer.gameObject);
			Destroy(treasure.gameObject);
		}
		BeginGame();
	}
}