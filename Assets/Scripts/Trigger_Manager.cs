using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Trigger_Manager : NetworkBehaviour {

  private InteractScript trig;
  private bool onTrigger;

  private Game_Manager_References GM_Ref;

  [SerializeField] private PlayerController m_Controller;

  void Start() {
    GM_Ref = GameObject.Find("Game Manager").GetComponent<Game_Manager_References>();
    onTrigger = false;
  }


  void Update() {
    if (isLocalPlayer) {
      if (onTrigger && trig != null && trig.withKey){
          if(Input.GetKeyDown(trig.GetKeyCode())){
            CmdTriggerInteract(GM_Ref.GetInteractObjectIndex(trig));
          }
      }
    }
  }

  [Command]
  void CmdTriggerInteract(int index) {
    GM_Ref.GetInteractObject(index).Interact();
  }

  void OnTriggerEnter(Collider other){
    if(isLocalPlayer) {
      if (other.tag.Equals("Interaction")) {
        SetTrig((InteractScript) other.GetComponent(typeof(InteractScript)));
        SetOnTrig(true);
        if(trig.withKey) trig.PreInteract();
        else CmdTriggerInteract(GM_Ref.GetInteractObjectIndex(trig));
      }
    }
  }

  void OnTriggerStay(Collider other){
      if(isLocalPlayer && other.tag.Equals("Interaction") && !onTrigger && trig != null){
          SetOnTrig(true);
          trig.PreInteract(); 
      }
  }
  void OnTriggerExit(Collider other){
      if(isLocalPlayer && other.tag.Equals("Interaction") && trig != null){
        SetOnTrig(false);
        trig.EndInteract();
        SetTrig(null);
      }
  }

  void SetOnTrig(bool newOnTrigger) {
    onTrigger = newOnTrigger;
/*    if ( (m_Controller.GetType()).GetMethod("SetOnTrig") != null )
      (m_Controller.GetType()).GetMethod("SetOnTrig").Invoke(m_Controller, (new object[] {onTrigger}));
    else Debug.Log("Method 'SetOnTrig' not found");*/
  }

  //Client Callbacks
  private void SetTrig(InteractScript t) {
    trig = t;
    /*if ( (m_Controller.GetType()).GetMethod("SetTrig") != null )
      (m_Controller.GetType()).GetMethod("SetTrig").Invoke(m_Controller, (new Object[] {trig}));
    else Debug.Log("Method 'SetTrig' not found");*/
    //CmdSetIndexValue(GM_Ref.GetInteractObjectIndex(t));
  }



}
