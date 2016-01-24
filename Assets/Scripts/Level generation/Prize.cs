using UnityEngine;
[System.Serializable]
public class Prize : MonoBehaviour {

	private MazeCell currentCell;

	// private MazeDirection currentDirection;

	public void SetLocation (MazeCell cell) {
		if (currentCell != null) {
			currentCell.OnPlayerExited();
		}
		currentCell = cell;
		transform.localPosition = cell.transform.localPosition;
		transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
		currentCell.OnPlayerEntered();
	}
}