using UnityEngine;

public class MazeCell : MonoBehaviour {

	public IntVector2 coordinates;

	public MazeRoom room;
	// public Transform cell;

	private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];

	private int initializedEdgeCount;

	public bool IsFullyInitialized {
		get {
			return initializedEdgeCount == MazeDirections.Count;
		}
	}

	public MazeDirection RandomUninitializedDirection {
		get {
			int skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount);
			for (int i = 0; i < MazeDirections.Count; i++) {
				if (edges[i] == null) {
					if (skips == 0) {
						return (MazeDirection)i;
					}
					skips -= 1;
				}
			}
			throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
		}
	}

	

	public MazeCellEdge GetEdge (MazeDirection direction) {
		return edges[(int)direction];
	}

	public void SetEdge (MazeDirection direction, MazeCellEdge edge) {
		edges[(int)direction] = edge;
		initializedEdgeCount += 1;
	}

	public void Show () {
		gameObject.SetActive(true);
	}

	public void Hide () {
		gameObject.SetActive(false);
	}

	public void OnPlayerEntered () {
		room.Show();
		for (int i = 0; i < edges.Length; i++) {
			edges[i].OnPlayerEntered();
		}
	}
	
	public void OnPlayerExited () {
		room.Hide();
		for (int i = 0; i < edges.Length; i++) {
			edges[i].OnPlayerExited();
		}
	}


	// public MazeCell (MazeRoom room, IntVector2 coordinates) {

	// 	MazeCell newCell = Instantiate(room.settings.cellPrefab) as MazeCell;
	// 	cells[coordinates.x, coordinates.z] = newCell;
	// 	newCell.coordinates = coordinates;
	// 	newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
	// 	newCell.transform.parent = transform;
	// 	newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);

	// 	newCell.transform.GetChild(0).GetComponent<Renderer>().material = room.settings.floorMaterial;
	// 	room.Add(newCell);
	// 	return newCell;
	// }
}