using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Door : TargetInteract{
  [SyncVar (hook = "SyncDirectionState")] private int direction = 1;

	public AudioClip doorOn_sound;
	public AudioClip doorOff_sound;

	private bool gravity = true;


	public void Start(){
    //GetComponent<Rigidbody>().velocity = transform.up * -3 * direction;
		GetComponent<Rigidbody>().useGravity = gravity;
	}

	public override string GetText(){
		return "open Door";
	}

	public override void Trigger(){
    if (hasAuthority) {
      gravity = !gravity;
      GetComponent<Rigidbody>().useGravity = false;
      GetComponent<Rigidbody>().velocity = transform.up * -3 * direction;
      direction = -direction;
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
    gravity = !gravity;
    GetComponent<Rigidbody>().useGravity = false;

    GetComponent<Rigidbody>().velocity = transform.up * -3 * direction;
    direction = newDirection;
  }

}
