using UnityEngine;
using System.Collections;

public class TutorialEnd : TriggeredObject{
  Rigidbody rb;

  public override void Trigger(Collider mummyCollider){
      Debug.Log("CHANGE SCENES");
  }
}
