using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Door : TargetInteract{
  [SyncVar (hook = "SyncDirectionState")] private int direction = 1;

  private void Start() {
    GetComponent<Rigidbody>().velocity = transform.up * -3 * direction;
  }

	public override string GetText(){
		return "open Door";
	}

	public override void Trigger(){
    if (hasAuthority) {
      direction = -direction;
      GetComponent<Rigidbody>().velocity = transform.up * -3 * direction;  
    } else {
      CmdSetDirection(-direction);
    }
	}

  [Command]
  private void CmdSetDirection(int newDirection) {
    direction = newDirection;
  }

  [Client]
  private void SyncDirectionState(int newDirection) {
    direction = newDirection;
    GetComponent<Rigidbody>().velocity = transform.up * -3 * direction;  
  }

}
