using UnityEngine;
using System.Collections;

public class CoffinOut : TriggeredObject{
  Rigidbody rb;
  bool finished;
  public bool triggered = false;

  public override void Trigger(Collider mummyCollider){
    finished = mummyCollider.gameObject.GetComponent<MummyController>().CheckTutorial();
    rb = gameObject.GetComponent<Rigidbody>();
    if(finished)
      rb.AddForce(transform.right * -100.0f, ForceMode.Force);
      triggered = true;
  }
}
