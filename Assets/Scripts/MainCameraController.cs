using UnityEngine;
using System.Collections.Generic;

public class MainCameraController : MonoBehaviour {

  [SerializeField] private Camera m_Camera;

  private int speed = 20;
  private Transform camTransform;
  // public Transform[] players;
  private List<Transform> testPos = new List<Transform>();
  private int currentPlayer;
  private readonly Vector3 distance = new Vector3(0, 3, 0);
	// Use this for initialization

  void Awake() {
    m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();

  }
	void Start () {
    camTransform = m_Camera.transform;
	}

  public void SetCameras(PlayerController[] playerList){
    testPos.Clear();
    foreach(PlayerController player in playerList){
      testPos.Add(player.transform);
    }
    Debug.Log(testPos.Count);
  }

  private void GotoNextPlayer(){
    currentPlayer++;
    if(currentPlayer >= testPos.Count) currentPlayer = 0;
    SetPosition(testPos[currentPlayer]);

  }

  private void GotoCurrentPlayer(){
    if( currentPlayer >= testPos.Count )  currentPlayer = 0;
    SetPosition(testPos[currentPlayer]);
  }

  private void GotoPreviousPlayer(){
    currentPlayer--;
    if(currentPlayer < 0) currentPlayer = testPos.Count-1;
    SetPosition(testPos[currentPlayer]);
  }


  void SetPosition(Transform t) {
    camTransform.position = t.position + distance - t.forward*5;
    camTransform.rotation = t.rotation;
  }
	// Update is called once per frame
	void Update () {
    speed  = ( Input.GetKey(KeyCode.LeftShift)) ? 40 : 20;
    if ( Input.GetKey(KeyCode.W)) camTransform.position += camTransform.forward*speed*Time.deltaTime;
    if ( Input.GetKey(KeyCode.S)) camTransform.position -= camTransform.forward*speed*Time.deltaTime;
    if ( Input.GetKey(KeyCode.D)) camTransform.position += camTransform.right*speed*Time.deltaTime;
    if ( Input.GetKey(KeyCode.A)) camTransform.position -= camTransform.right*speed*Time.deltaTime;
    if ( Input.GetKey(KeyCode.Q)) camTransform.position += camTransform.up*speed*Time.deltaTime;
    if ( Input.GetKey(KeyCode.E)) camTransform.position -= camTransform.up*speed*Time.deltaTime;

    if ( testPos != null && testPos.Count != 0) {
      if( Input.GetKeyDown(KeyCode.P)) GotoNextPlayer();
      if( Input.GetKeyDown(KeyCode.O)) GotoCurrentPlayer();
      if( Input.GetKeyDown(KeyCode.I)) GotoPreviousPlayer();
    }
	}

}
