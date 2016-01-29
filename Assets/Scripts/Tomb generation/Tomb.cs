using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
public class Tomb : NetworkBehaviour {

	public int roomCount;
	public TombRoom roomPrefab;

	private TombRoom[] rooms;

	public void Generate(){
		// initialize rooms
		rooms = new TombRoom[roomCount];

		// create rooms
		for(int i=0; i<roomCount; i++){
			rooms[i] = CreateRoom();
		}

		//arrange rooms
	}

	private TombRoom CreateRoom(){
		TombRoom newRoom = Instantiate(roomPrefab) as TombRoom;
		
		newRoom.Initialize();
		newRoom.transform.parent = transform;
		return newRoom;
	}
}

