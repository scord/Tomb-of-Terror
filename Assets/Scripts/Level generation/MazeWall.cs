using UnityEngine;

public class MazeWall : MazeCellEdge {

	public Transform wall;

	public void Initialize (MazeCell cell, MazeCell otherCell, MazeDirection direction, float height) {
		base.Initialize (cell, otherCell, direction);
		wall.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
		Vector3 scaleV = new Vector3(1.0f, height, 0.05f);

		wall.localScale = scaleV;
		// Debug.Log(wall.localScale);
	}
}