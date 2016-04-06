using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class Level : MonoBehaviour {
  public PassagePart passage;
  public PassagePart[] corners;
  public LevelRoom roomPrefab;
  public int roomCount = 1;


  private PassagePart[] passages;
  private LevelRoom[] rooms;
  private List<LevelTunnel> tunnels = new List<LevelTunnel>();

  void Start(){
    rooms = new LevelRoom[roomCount];

    int linkNr;
    for(int i=0; i<roomCount; i++){
      rooms[i] = CreateRoom();

      if(i > 0){
        linkNr = Random.Range(0, i);
        tunnels.Add(ConnectRooms(rooms[i], rooms[linkNr]));
      }
    }
    PositionRooms();
    for(int i=0; i<roomCount; i++)
      rooms[i].FinishRoom();
  }

  private void PositionRooms(){
    bool top = true,
         vertical = false,
         previousVertical;
    int distance;
        // previousRight = 0,
    float newX = 0,
          newY = 0,
          newZ = 0;
    MazeDirection[] directions;
    for(int i=1; i<roomCount; i++){

      previousVertical = vertical;
      vertical = Random.Range(0,2) == 1 ? true : false;
      distance = Random.Range(0, 5);

      if(!vertical){
        newX += rooms[i-1].Size().x + 17*distance;
        rooms[i-1].AddDoor(MazeDirection.West);
        rooms[i].AddDoor(MazeDirection.East);
        directions = new MazeDirection[] {MazeDirection.West, MazeDirection.West};
        rooms[i].transform.localPosition += new Vector3(newX, newY, newZ);
        ConnectPaths(rooms[i-1].DoorPosition(MazeDirection.West), rooms[i].DoorPosition(MazeDirection.East), directions);
      }
      else{
        if (!previousVertical)
            top = Random.Range(0,2) == 1 ? true : false;

        if(top){
          directions = new MazeDirection[] {MazeDirection.North, MazeDirection.North};

          newZ -= rooms[i].Size().z + 17*distance;
          rooms[i-1].AddDoor(MazeDirection.North);
          rooms[i].AddDoor(MazeDirection.South);
          rooms[i].transform.localPosition += new Vector3(newX, newY, newZ);
          ConnectPaths(rooms[i-1].DoorPosition(MazeDirection.North), rooms[i].DoorPosition(MazeDirection.South), directions);
        }
        else{
          directions = new MazeDirection[] {MazeDirection.South, MazeDirection.South};
          newZ += rooms[i].Size().z + 17*distance;
          rooms[i-1].AddDoor(MazeDirection.South);
          rooms[i].AddDoor(MazeDirection.North);
          rooms[i].transform.localPosition += new Vector3(newX, newY, newZ);
          ConnectPaths(rooms[i-1].DoorPosition(MazeDirection.South), rooms[i].DoorPosition(MazeDirection.North), directions);
        }
      }

      newX = rooms[i].transform.localPosition.x;

    }
  }

  LevelRoom CreateRoom(){
    LevelRoom newRoom = Instantiate(roomPrefab) as LevelRoom;
    newRoom.Initialize();

    return newRoom;
  }

  // create a passage between any 2 rooms
  LevelTunnel ConnectRooms(LevelRoom room1, LevelRoom room2){
    LevelTunnel tunnel = new LevelTunnel();

    // set tunnel parameters
    tunnel.Initialize(room1, room2);

    // set rooms parameters
    room1.AddTunnel(tunnel);
    room2.AddTunnel(tunnel);

    return tunnel;
  }

  void ConnectPaths(Vector3 start, Vector3 final, MazeDirection[] direction){
    Debug.Log("Start position" + start);
    Debug.Log("Final position" + final);
    MazeDirection oldDirection = direction[0];
    int count = 0;

    switch(direction[0]){
      case MazeDirection.North:
        start -= new Vector3(0, 0, 9f);
        break;
      case MazeDirection.South:
        start += new Vector3(0, 0, 9f);
        break;
      case MazeDirection.East:
        start -= new Vector3(8f, 0, 0);
        break;
      case MazeDirection.West:
        start += new Vector3(9f, 0, 0);
        break;
    }
    Vector3 location = start;

    // Debug.Log(checkPosition(location, final, direction[0]));
    for(int i = 0; i < direction.Length; i++){
      if(! (oldDirection == direction[i]))
        location = location + MakeCorner(location, oldDirection, direction[i]);
        count = 0;
      while(!checkPosition(location, final, direction[i])){
        location = location + MakePath(location, direction[i]);
        count++;
      }
    }
  }

  bool checkPosition(Vector3 current, Vector3 final, MazeDirection direction){
    Debug.Log(current);
    Debug.Log(final);
    Debug.Log(direction);
    switch (direction){
      case MazeDirection.North:
        return (current.z < final.z);
        break;
      case MazeDirection.South:
        return (current.z > final.z);
        break;
      case MazeDirection.East:
        return (current.x < final.x);
        break;
      case MazeDirection.West:
        return (current.x > final.x);
        break;
    }
    return false;
  }

  void GeneratePath(Vector3 start, Vector3 final, MazeDirection direction) {
    Vector3 location = new Vector3(0,0,0);
    MazeDirection newDirection = direction;
    for(int i= 0; i< 10; i++){

      if((int)newDirection != (int)direction )
          location = location + MakeCorner(location, direction, newDirection);
      else
        location = location + MakePath(location, direction);

      direction = newDirection;
      // newDirection = MazeDirections.RandomValue(direction);
      Debug.Log(newDirection);
    }
  }

  private Vector3 MakeCorner(Vector3 location, MazeDirection direction, MazeDirection newDirection){
    PassagePart path = null;

    // north
    if( direction == MazeDirection.North){
      if(newDirection == MazeDirection.East)
        path = Instantiate(corners[0]) as PassagePart;
      else
        path = Instantiate(corners[1]) as PassagePart;
    }
    // south
    if( direction == MazeDirection.South){
      if(newDirection == MazeDirection.East)
        path = Instantiate(corners[2]) as PassagePart;
      else
        path = Instantiate(corners[3]) as PassagePart;
    }

    // East
    if( direction == MazeDirection.East){
      if(newDirection == MazeDirection.South)
        path = Instantiate(corners[1]) as PassagePart;
      else
        path = Instantiate(corners[3]) as PassagePart;
    }

    // West
    if( direction == MazeDirection.West){
      if(newDirection == MazeDirection.South)
        path = Instantiate(corners[0]) as PassagePart;
      else
        path = Instantiate(corners[2]) as PassagePart;
    }

    path.transform.localPosition = location;
    Vector3 nextPos = path.getNextPos(newDirection);

    return nextPos;
  }

  private Vector3 MakePath(Vector3 location, MazeDirection direction) {
    PassagePart path = Instantiate(passage) as PassagePart;
    path.transform.localPosition = location;


    Debug.Log(path.getNextPos(direction));
    Vector3 nextPos = path.getNextPos(direction);

    switch(direction) {
      case MazeDirection.South:
      case MazeDirection.North:
        // by default it is on the North-South direction
        break;
      case MazeDirection.East:
      case MazeDirection.West:
        path.transform.eulerAngles = new Vector3(0, 90, 0);
        break;
    }
    return nextPos;
  }

}
