using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
public class TombRoom : NetworkBehaviour {

	public IntVector2 size;
	public TombCell CellPrefab;
	public TombPassage[] passages;

	private TombCell[,] cells;

	public void Initialize(){

		// set the room dimensions
		this.size.x = Random.Range((this.size.x)/2, this.size.x);
		this.size.z = Random.Range((this.size.z/2), this.size.z);
		cells = new TombCell[size.x, size.z];

		Debug.Log("X size: " + this.size.x  + " Z size: " + this.size.z );

		// create cells in room
		for(int i=0; i<size.x; i++)
			for(int j=0; j<size.z; j++){
				cells[i,j] = CreateCell(new IntVector2(i,j));
			}
	}

	public TombCell CreateCell(IntVector2 coordinates){
		TombCell newCell = Instantiate(CellPrefab) as TombCell;

		newCell.transform.parent = transform;
		newCell.name = "TombCell " + coordinates.x + ", " + coordinates.z;
		newCell.transform.localPosition = new Vector3(coordinates.x + 0.5f, 0, coordinates.z + 0.5f);
		return newCell;
	}
}

