using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class LevelRoom : MonoBehaviour {
  public GameObject ceilingPrefab;
  public GameObject wallPrefab;
  public GameObject doorWallPrefab;
  public GameObject[] roomElements;

  private const int matrixL = 10;
  private bool[,] matrix;
  private int placedObjects = 0;
  private GameObject[] edges;
  private bool[] walls;
  private bool[] doors;
  private List<LevelTunnel> tunnels = new List<LevelTunnel>();
  private Vector3 size;

  public void Initialize(){
    walls = new bool[4];
    doors = new bool[4];
    edges = new GameObject[4];
    for(int i=0; i<walls.Length; i++){
      walls[i] = false;
      doors[i] = false;
    }

    // initialise matrix
    matrix = new bool[matrixL,matrixL];
    for(int i=0; i < matrixL; i++ )
      for(int j=0; j<matrixL; j++){
        if(i==0 || i == matrixL-1 || j == 0 || j == matrixL-1)
          matrix[i,j] = true;
        else
          matrix[i,j] = false;
      }

    // set local position to 0
    transform.localPosition = new Vector3(0, 0, 0);

    // make ceiling and floor
    GameObject ceiling = Instantiate(ceilingPrefab) as GameObject;
    GameObject floor = Instantiate(ceilingPrefab) as GameObject;
    ceiling.transform.parent = transform;
    floor.transform.parent = transform;
    floor.transform.localPosition -= new Vector3(0, ceiling.transform.localPosition.y+0.5f, 0);
    this.size = ceiling.transform.localScale;
    FinishRoom();

  }

  public void FinishRoom(){
    for(int i=0; i<4; i++){
      if(!walls[i]){
        GameObject wall = Instantiate(wallPrefab) as GameObject;
        wall.transform.parent = transform;
        Rotate(wall, (MazeDirection)i);
        PlaceWall(wall, (MazeDirection)i);
        edges[i] = wall;
      }
    }
    CustomiseRoom();
  }

  private void CustomiseRoom(){
    PlacePylons();
  }

  private void PlacePylons(){
    // place objects
    int nrOfObjects = Random.Range(5,20);
    int x, z;
    while(placedObjects < nrOfObjects){
      x = Random.Range(0,10);
      z = Random.Range(0,10);
      Place(roomElements[0], x, z);
    }
  }

  private void Place(GameObject prefab, int x, int z){
    if(!matrix[x,z]){
      GameObject element = Instantiate(prefab) as GameObject;
      element.transform.parent = transform;
      element.transform.localPosition = new Vector3(+size.x/2 - x*size.x/matrixL, 0, -size.z/2 + z*size.z/matrixL);
      placedObjects++;
      matrix[x,z] = true;
      matrix[x,z+1] = true;
      matrix[x+1, z] = true;
      matrix[x-1, z] = true;
      matrix[x, z-1] = true;
    }
  }

  private void PlaceWall(GameObject obj, MazeDirection direction){
    switch(direction){
      case MazeDirection.North:
        obj.transform.localPosition = new Vector3(0, 0, -(size.z - obj.transform.localScale.z)/2);
        break;
      case MazeDirection.South:
        obj.transform.localPosition = new Vector3(0, 0, (size.z - obj.transform.localScale.z)/2);
        break;
      case MazeDirection.East:
        obj.transform.localPosition = new Vector3(-(size.x - obj.transform.localScale.z)/2, 0, 0);
        break;
      case MazeDirection.West:
        obj.transform.localPosition = new Vector3((size.x - obj.transform.localScale.z)/2, 0, 0);
        break;
    }
  }

  public Vector3 DoorPosition(MazeDirection direction){
    return edges[(int)direction].transform.position;
  }



  private void  Rotate(GameObject obj, MazeDirection direction){
    switch(direction){
      case MazeDirection.North:
        obj.transform.eulerAngles = new Vector3(0, 0, 0);
        break;
      case MazeDirection.South:
        obj.transform.eulerAngles = new Vector3(0, 180, 0);
        break;
      case MazeDirection.East:
        obj.transform.eulerAngles = new Vector3(0, 90, 0);
        break;
      case MazeDirection.West:
        obj.transform.eulerAngles = new Vector3(0, -90, 0);
        break;
    }
  }

  public void AddDoor(MazeDirection direction){
    GameObject door = Instantiate(doorWallPrefab) as GameObject;
    edges[(int)direction] = door;
    door.transform.parent = transform;
    Rotate(door, direction);
    PlaceWall(door, direction);
    walls[(int)direction] = true;
    doors[(int)direction] = true;
  }

  public void AddTunnel(LevelTunnel tunnel){
    this.tunnels.Add(tunnel);
  }

  public Vector3 Size(){
    return this.size;
  }
}
