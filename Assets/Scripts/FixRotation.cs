using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
 
public class FixRotation : NetworkBehaviour
{
    [SyncVar (hook = "UpdateInitialRotation")] Vector3 rotation;
    private Vector3 initialRotation;
 
    public override void OnStartServer()      
    {
      initialRotation = transform.rotation.eulerAngles;
    }

    public override void OnStartLocalPlayer() {
      base.OnStartLocalPlayer();
      CmdRequestRotationUpdate();
    }

    [Command]
    void CmdRequestRotationUpdate() {
      rotation = initialRotation;
    }

    [Client]
    void UpdateInitialRotation(Vector3 newValue) {
      rotation = newValue;
      transform.rotation = Quaternion.Euler(rotation);
    }

}