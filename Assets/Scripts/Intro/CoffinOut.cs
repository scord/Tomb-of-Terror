using UnityEngine;
using System.Collections;

public class CoffinOut : TriggeredObject{
  Rigidbody rb;

  public override void Trigger(Collider mummyCollider){
    rb = gameObject.GetComponent<Rigidbody>();
    rb.AddForce(transform.right * -1000.0f, ForceMode.Force);
  }
}
