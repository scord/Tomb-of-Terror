// Source:
// Firoball code on
// http://forum.unity3d.com/threads/spawning-players-via-networkmanager-ignores-rotation.349089/

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
 
public class FixRotation : NetworkBehaviour
{
    [SyncVar] Vector3 rotation;
 
    public override void OnStartServer()      
    {
      rotation = transform.rotation.eulerAngles;
    }
 
    public override void OnStartClient()
    {
      transform.rotation = Quaternion.Euler(rotation);
    }
}