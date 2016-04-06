using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class PassagePart : MonoBehaviour {

  public Vector3 getNextPos(MazeDirection direction) {
    Transform ceil  = this.transform.Find("Ceiling").transform;
    Transform wall = this.transform.Find("Wall").transform;
    switch (direction) {
      case MazeDirection.North:
        return new Vector3(0, 0,  -ceil.localScale.z );
        break;
      case MazeDirection.South:
        return new Vector3(0, 0,  ceil.localScale.z );
        break;
      case MazeDirection.East:
        return new Vector3( -ceil.localScale.x, 0 , 0);
        break;
      case MazeDirection.West:
        return new Vector3( ceil.localScale.x, 0 , 0);
        break;
    }
    return ceil.localScale;
  }

}
