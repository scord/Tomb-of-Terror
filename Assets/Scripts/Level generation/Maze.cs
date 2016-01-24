using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {

	public IntVector2 size;
	public IntVector2 floorSize;
	public int nrOfFloors;


	public MazeWall[] prefabsWall;
	public MazeCell[] topPrefab;

	public MazeCell cellPrefab;
	public MazePassage passagePrefab;
	public MazeDoor doorPrefab;

	[Range(0f, 1f)]
	public float doorProbability;

	[Range(0f, 1f)]
	public float floorProbability;

	public MazeWall[] wallPrefabs;
	public MazeRoomSettings[] roomSettings;
	public MazeRoomSettings[] topRoomSettings;	

	private MazeCell[,] cells;
	private MazeCell[,] topCells;

	private List<MazeRoom> rooms = new List<MazeRoom>();

	public IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
		}
	}

	public bool ContainsCoordinates (IntVector2 coordinate) {
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
	}

	public MazeCell GetCell (IntVector2 coordinates, bool top) {
		if(top){
			return topCells[coordinates.x, coordinates.z];
		}
		else{
			return cells[coordinates.x, coordinates.z];
			
		}
	}
	public MazeCell GetTopCell(IntVector2 coordinates){
		return topCells[coordinates.x, coordinates.z];
	}

	public void Generate () {
		// WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
		cells = new MazeCell[size.x, size.z];
		List<MazeCell> activeCells = new List<MazeCell>();
		DoFirstGenerationStep(activeCells, false);
		while (activeCells.Count > 0) {
			// yield return delay;
			DoNextGenerationStep(activeCells, false);
		}

		topCells = new MazeCell[size.x, size.z];
		List<MazeCell> activeTopCells = new List<MazeCell>();
		DoFirstGenerationStep(activeTopCells, true);
		int c = 0;
		while( activeTopCells.Count > 0){
			c++;	
			DoNextGenerationStep(activeTopCells, true);
			Debug.Log(activeTopCells.Count);
		}


	}

	private void DoNextGenerationStepTop (List<MazeCell> activeTopCells) {
		// int currentIndex = activeTopCells.Count - 1;
		// MazeCell currentTopCell = activeTopCells[currentIndex];

		// if (currentTopCell.IsFullyInitialized) {
		// 	activeTopCells.RemoveAt(currentIndex);
		// 	return;
		// }

		// MazeDirection direction = currentTopCell.RandomUninitializedDirection;
		// IntVector2 coordinates = currentTopCell.coordinates + direction.ToIntVector2();
		// if (ContainsCoordinates(coordinates)) {
	 //    	Debug.Log("containsCoordinates");
		// 	MazeCell neighbor = GetTopCell(coordinates);
		// 	if (neighbor == null) {
		// 		Debug.Log("neighbor == null");
		// 		neighbor = CreateTopCell(coordinates);
		// 		CreatePassage(currentTopCell, coordinates, direction, );
		// 		activeTopCells.Add(neighbor);
		// 	}
		// 	else if (currentTopCell.room.settingsIndex == neighbor.room.settingsIndex) {
		// 		CreatePassageInSameRoom(currentTopCell, neighbor, direction);
		// 		Debug.Log("Same room Passage");
		// 	}
		// 	else {
		// 		CreateWall(currentTopCell, neighbor, direction);
		// 		Debug.Log("Wall");
		// 	}
		// }
		// else {
		// 	CreateWall(currentTopCell, null, direction);
		// 	Debug.Log("OuterWall");
		// }
	}

	private void DoFirstGenerationStep (List<MazeCell> activeCells, bool top) {
		
		MazeCell newCell = Initialize(CreateRoom(-1, top), RandomCoordinates, top);
		activeCells.Add(newCell);
	}

	private void DoNextGenerationStep (List<MazeCell> activeCells, bool top) {

		for(int i=0; i<activeCells.Count-1; i++){
			if(activeCells[i].IsFullyInitialized){
				activeCells.RemoveAt(i);
				i--;
			}
		}

		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells[currentIndex];
		if (currentCell.IsFullyInitialized) {
			activeCells.RemoveAt(currentIndex);
			return;
		}
		MazeDirection direction = currentCell.RandomUninitializedDirection;
		IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
		if (ContainsCoordinates(coordinates)) {
			MazeCell neighbor = GetCell(coordinates, top);
			if (neighbor == null) {
				neighbor = CreatePassage(currentCell, coordinates, direction, top);
				activeCells.Add(neighbor);
			}
			else if (currentCell.room.settingsIndex == neighbor.room.settingsIndex) {
				CreatePassageInSameRoom(currentCell, neighbor, direction);
			}
			else {
				CreateWall(currentCell, neighbor, direction);
			}
		}
		else {
			CreateWall(currentCell, null, direction);
		}
	}

	private MazeCell CreateTopCell(IntVector2 coordinates){
		MazeCell newCell = Instantiate(topPrefab[0]) as MazeCell;
		topCells[coordinates.x, coordinates.z] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Top Cell " + coordinates.x + ", " + coordinates.z;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 2f, coordinates.z - size.z * 0.5f + 0.5f);
		return newCell;
	}

	private MazeCell CreatePassage (MazeCell cell, IntVector2 coordinates, MazeDirection direction, bool top) {
		MazePassage prefab = Random.value < doorProbability ? doorPrefab : passagePrefab;
		MazePassage passage = Instantiate(prefab) as MazePassage;
			MazeCell otherCell;
		if (passage is MazeDoor) {
			otherCell = Initialize(CreateRoom(cell.room.settingsIndex, top), coordinates, top);
			passage.Initialize(cell, otherCell, direction);
		}
		else {
			otherCell = Initialize(cell.room, coordinates, top);
			passage.Initialize(cell, otherCell, direction);
		}
		passage.Initialize(otherCell, cell, direction.GetOpposite());
		return otherCell;
	}

	private void CreatePassageInSameRoom (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazePassage passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage.Initialize(otherCell, cell, direction.GetOpposite());
		if (cell.room != otherCell.room) {
			MazeRoom roomToAssimilate = otherCell.room;
			cell.room.Assimilate(roomToAssimilate);
			rooms.Remove(roomToAssimilate);
			Destroy(roomToAssimilate);
		}
	}

	private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazeWall wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
		wall.Initialize(cell, otherCell, direction);
		if (otherCell != null) {
			wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
			wall.Initialize(otherCell, cell, direction.GetOpposite());
		}
	}

	private MazeRoom CreateRoom (int indexToExclude, bool top) {
		MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
		newRoom.settingsIndex = (top) ? Random.Range(0, topRoomSettings.Length) : Random.Range(0, roomSettings.Length);
		if (newRoom.settingsIndex == indexToExclude) {
			newRoom.settingsIndex = (top) ?  ((newRoom.settingsIndex + 1) % topRoomSettings.Length) :  ((newRoom.settingsIndex + 1) % roomSettings.Length);
		}
		newRoom.settings = (top) ? topRoomSettings[newRoom.settingsIndex] : roomSettings[newRoom.settingsIndex];
		rooms.Add(newRoom);
		return newRoom;
	}

	private MazeCell Initialize (MazeRoom room, IntVector2 coordinates, bool top) {
		MazeCell newCell = CreateCell(coordinates, room, top);
		newCell.transform.GetChild(0).GetComponent<Renderer>().material = room.settings.floorMaterial;
		room.Add(newCell);
		return newCell;
	}

	private MazeCell CreateCell (IntVector2 coordinates, MazeRoom room, bool top) {
		MazeCell newCell = Instantiate(room.settings.cellPrefab) as MazeCell;
		newCell.coordinates = coordinates;
		newCell.transform.parent = transform;
		if(!top){
			cells[coordinates.x, coordinates.z] = newCell;
			newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
			newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
		}
		else{
			topCells[coordinates.x, coordinates.z] = newCell;
			newCell.name = "Top Cell " + coordinates.x + ", " + coordinates.z;
			newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 2f, coordinates.z - size.z * 0.5f + 0.5f);

		}
		return newCell;
	}
}

