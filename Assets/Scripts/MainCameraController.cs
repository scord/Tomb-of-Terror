using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainCameraController : MonoBehaviour {

  [SerializeField] private Camera m_Camera;

  private int speed = 20;
  private Transform camTransform;
  // public Transform[] players;
  private GameObject m_Explorer;
  private Player_SyncPoints m_SyncPointsScript;
  private Player_SyncHealth m_SyncHealthScript;
  private List<Transform> testPos = new List<Transform>();
  private int currentPlayer;
  private readonly Vector3 distance = new Vector3(0, 3, 0);
	private Player_SyncPoints points;
  private Player_SyncHealth lives;
  [SerializeField] private AirManager m_AirManager;


  [SerializeField] private Text livesCanvas;
  [SerializeField] private Text pointsCanvas;
  [SerializeField] private Text timerCanvas;

  void Awake() {
    m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
  }

  void UpdateAir(int newAir){
    timerCanvas.text = newAir.ToString();
  }

	void Start () {
    camTransform = m_Camera.transform;
    if(m_AirManager != null)
      m_AirManager.EventAirUpdate += UpdateAir;
	}

  void OnDisable(){
    if(m_AirManager!=null)
      m_AirManager.EventAirUpdate -= UpdateAir;
  }

  public void SetCameras(PlayerController[] playerList){
    testPos.Clear();
    foreach(PlayerController player in playerList){
      testPos.Add(player.transform);
      if(player.gameObject.tag == "Explorer") {
        m_Explorer = player.gameObject;
        m_SyncHealthScript = player.GetComponent<Player_SyncHealth>();
        m_SyncPointsScript = player.GetComponent<Player_SyncPoints>();
      }
    }
  }

  private void GotoNextPlayer(){
    currentPlayer++;
    if(currentPlayer >= testPos.Count) currentPlayer = 0;
    SetPosition(testPos[currentPlayer]);

  }

  private void GotoCurrentPlayer(){
    if( currentPlayer >= testPos.Count )  currentPlayer = 0;
    SetPosition(testPos[currentPlayer]);
    Debug.Log("orice");
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
      if( Input.GetKeyDown(KeyCode.Alpha1)) GotoNextPlayer();
      if( Input.GetKeyDown(KeyCode.Alpha2)) GotoCurrentPlayer();
      if( Input.GetKeyDown(KeyCode.Alpha3)) GotoPreviousPlayer();
    }

    if(m_Explorer != null) {
      if(pointsCanvas != null && m_SyncPointsScript != null){
        pointsCanvas.text = m_SyncPointsScript.pointsEarned + "/" + m_SyncPointsScript.necessaryPoints;
      }
      if(livesCanvas != null &&  m_SyncHealthScript != null){
        livesCanvas.text = m_SyncHealthScript.lives.ToString();
      }   
    }
	}

}
