using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
public class TombPassage : NetworkBehaviour {
	public TombRoom room1;
	public TombRoom room2;

	public void Initialize(TombRoom first, TombRoom second){
		this.room1 = first;
		this.room2 = second;
	}

}

