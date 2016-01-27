using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
public class Maze : NetworkBehaviour {

	public IntVector2 size;

    [SyncVar]
    public int seed = 42;

	public float roomHeight = 3f;
	private float levelScale = 3;

	// public MazeCell cellPrefab;
	public MazePassage passagePrefab;
	public MazeDoor bottom_doorPrefab;
	public MazeDoor[] top_doorPrefab;
	public Material defaultMaterial;

	[Range(0f, 1f)]
	public float doorProbability;

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

	public void Generate () {
        Random.seed = seed;
		// WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
		cells = new MazeCell[size.x, size.z];
		List<MazeCell> activeCells = new List<MazeCell>();
		DoFirstGenerationStep(activeCells, false);
		int c = 0;
		while (activeCells.Count > 0 ) {
			c++;
			DoNextGenerationStep(activeCells, false);
		}

		topCells = new MazeCell[size.x, size.z];
		List<MazeCell> activeTopCells = new List<MazeCell>();
		DoFirstGenerationStep(activeTopCells, true);
	    c = 0;
		while( activeTopCells.Count > 0){
			c++;	
			DoNextGenerationStep(activeTopCells, true);
			//Debug.Log(activeTopCells.Count);
		}

		//rescale maze
        transform.localScale = new Vector3(levelScale, levelScale, levelScale);

        // generate roof
        GameObject rooftop = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rooftop.transform.position = new Vector3(0, 2*roomHeight*levelScale, 0);
        rooftop.transform.localScale = new Vector3(size.x*levelScale, 0.1f, size.z*levelScale);
        rooftop.GetComponent<Renderer>().material = defaultMaterial;
        rooftop.name = "Ceiling";
        rooftop.transform.parent = this.transform;

	}

	private void DoFirstGenerationStep (List<MazeCell> activeCells, bool top) {
		
		MazeCell newCell = Initialize(CreateRoom(-1, top), RandomCoordinates, top);
		activeCells.Add(newCell);
	}

	private void DoNextGenerationStep (List<MazeCell> activeCells, bool top) {



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
		for(int i=0; i<activeCells.Count-1; i++){
			if(activeCells[i].IsFullyInitialized){
				activeCells.RemoveAt(i);
				i--;
			}
		}
	}

	private MazeCell CreatePassage (MazeCell cell, IntVector2 coordinates, MazeDirection direction, bool top) {
		MazePassage prefab = Random.value < doorProbability ? bottom_doorPrefab : passagePrefab;
		if(top){
			bool sameRoom = Random.value < doorProbability ? false : true;
			if(sameRoom)
				prefab = passagePrefab;	
			else {
				bool stairs = true;
				if(cell.emptyCheck()){

					IntVector2 tempLoc = new IntVector2(cell.coordinates.x, cell.coordinates.z);
					MazeCell tempCell = GetCell( tempLoc, false);
					tempCell = neighborCell(tempCell, direction.GetOpposite());

					if(null != tempCell)
						for(int i=0; i<2 && stairs; i++){
							if(tempCell == null || tempCell.wallCheck(direction.GetOpposite()))
								stairs = false;
							tempCell = neighborCell(tempCell, direction.GetOpposite());

						}
					else stairs = false;
				}
				else
				 stairs = false;
					
			prefab = stairs ? top_doorPrefab[1] : top_doorPrefab[0];
			if(stairs) Debug.Log("Stairs");
			}
		}

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
		cell.wallCheck(direction);
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
		wall.Initialize(cell, otherCell, direction, roomHeight);
		if (otherCell != null) {
			wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
			wall.Initialize(otherCell, cell, direction.GetOpposite(), roomHeight);
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
			newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, roomHeight, coordinates.z - size.z * 0.5f + 0.5f);

		}
		return newCell;
	}

	private MazeCell neighborCell(MazeCell cell, MazeDirection direction){
		MazeCell neighbor;
		IntVector2 dirVec = direction.ToIntVector2();
		
		// Debug.Log( "direction x: " + dirVec.x + " direction z: " + dirVec.z);
		// Debug.Log("cell .x: " + cell.coordinates.x + "cell .z: " +cell.coordinates.z);
		
		IntVector2 neighborCoordinates = cell.coordinates + direction.ToIntVector2();
		if(ContainsCoordinates(neighborCoordinates))
		    neighbor = GetCell(neighborCoordinates, false);
		else
		    neighbor = null;
		return neighbor;
	}
}

