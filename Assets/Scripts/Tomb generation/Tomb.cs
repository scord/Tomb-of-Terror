using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
public class Tomb : NetworkBehaviour {

	public int roomCount;
	public TombRoom roomPrefab;

	private TombRoom[] rooms;
	private List<TombPassage> passages = new List<TombPassage>();

	public void Generate(){
		// initialize rooms
		rooms = new TombRoom[roomCount];

		// create rooms
		int linkNr;
		for(int i=0; i<roomCount; i++){
			rooms[i] = CreateRoom();
			
			if(i>0){
				// link room to one of previous rooms randomly
				linkNr = Random.Range(0, i);
				passages.Add(ConnectRooms(rooms[i], rooms[linkNr]));
			}
		}

		// arrange rooms
		PositionRooms();
	}

	private void PositionRooms(){
		bool top = true;
		bool vertical = true;
		int distance;
		int previousRight = 0, 
				previousVertical = 0;
		float newX=0, newY=0, newZ=0;

		for(int i=1; i<roomCount; i++){
			vertical = Random.Range(0, 2) == 1 ? true : false;
			distance = Random.Range(2, 7);

			// 4 cases : top-vertical, !top-vertical, top-right, !top-right

			if(!vertical){
				newX = newX + rooms[i-1].size.x + distance;
				centerVertically(rooms[i-1], rooms[i]);
				createPassage(rooms[i-1], rooms[i], "X");

			}
			else{
				if(top) {
					newZ = newZ -rooms[i].size.z - distance;
				}
				else {
					newZ = newZ + rooms[i].size.z + distance;
				}
			  centerHorizontally(rooms[i-1], rooms[i]);
			  createPassage(rooms[i-1], rooms[i], "Z");
			}
			rooms[i].transform.localPosition += new Vector3(newX, newY, newZ);

				newX = rooms[i].transform.localPosition.x;
		}
	}

	private void centerHorizontally(TombRoom room1, TombRoom room2){
		float dif = Mathf.Abs(room1.size.x - room2.size.x);
		if(dif != 0)
		if(room1.size.x < room2.size.x) room2.transform.localPosition += new Vector3(-dif/2, 0, 0 );
		else room2.transform.localPosition += new Vector3(dif/2, 0, 0 );
	}

	private void centerVertically(TombRoom room1, TombRoom room2){
		float dif = Mathf.Abs(room1.size.z - room2.size.z);
		if(dif != 0)
		if(room1.size.z < room2.size.z) room2.transform.localPosition += new Vector3(0, 0, -dif/2 );
		else room2.transform.localPosition += new Vector3(0, 0, dif/2 );
	}

	private TombRoom CreateRoom(){
		TombRoom newRoom = Instantiate(roomPrefab) as TombRoom;
		
		newRoom.Initialize();
		newRoom.transform.parent = transform;
		return newRoom;
	}

	private void createPassage(TombRoom room1, TombRoom room2, string axis){
		switch (axis){
			case "X": 
				IntVector2 passEnd = new IntVector2(0, room2.size.z/2);
				IntVector2 passBeg = new IntVector2(room1.size.x-1, room1.size.z/2);
				Debug.Log("0 si " +  room2.size.z/2 + " cu " + (room1.size.x-1) + " si " + room1.size.z/2);
				GeneratePath(room1.GetCell(passBeg), room2.GetCell(passEnd));
				break;

			case "Z": 
				Debug.Log("Z axis");
				break;
		}
	}

	// creates pathway between 2 cells
	private void GeneratePath(TombCell cell1, TombCell cell2){

	}

	private TombPassage ConnectRooms(TombRoom room1, TombRoom room2){
		TombPassage passage = new TombPassage();
		passage.Initialize(room1, room2);

		room1.passages.Add(passage);
		room2.passages.Add(passage);
		return passage;
	}
}

