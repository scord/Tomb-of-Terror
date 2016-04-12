using UnityEngine;
using System.Collections;

public class CoffinEnter : TriggeredObject{
  private SoundVision vision;
  private GameObject mummy;
  private Camera view;

  public override void Trigger(Collider mummyCollider){
    mummy = mummyCollider.gameObject.transform.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").gameObject;
    vision = mummy.GetComponent<SoundVision>();
    view = mummy.GetComponent<Camera>();

    gameObject.GetComponent<Rigidbody>().useGravity = true;

    vision.enabled = true;
    view.backgroundColor = new Color (0, 0, 0, 1);
    view.cullingMask = (view.cullingMask ) &  ~(1 << LayerMask.NameToLayer("Ignore Sound Vision"));
  }
}
