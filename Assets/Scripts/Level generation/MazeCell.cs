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

	public bool emptyCheck() {
		if(null == this.transform.Find("Floor"))
			return true;
		return false;
	}

	public bool wallCheck(MazeDirection direction){
		if(this.edges[(int)direction] is MazeWall)
			return true;
		return false;
	}
}