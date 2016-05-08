using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_SyncPoints : NetworkBehaviour {

  [SyncVar (hook="OnPointsEarnedUpdate")] public int m_PointsEarned = 0;
  [SerializeField] private int m_NecessaryPoints = 1000; //default
  [SerializeField] private int m_DefaultValuePoints = 400; //default
  [SerializeField] private Pickup_Manager m_PickupManager;
  [SerializeField] private PlayerController m_PlayerController;
  [SerializeField] private GameObject m_PointsCanvas; //Put canvas in prefab
//  private Text m_PointsMessage; 

	[SerializeField] private Text m_PointsMessage; 
	[SerializeField] private Text points; 		// score
	private double timer;

  public int necessaryPoints { get { return m_NecessaryPoints;}}
  public int defaultValuePoints { get { return m_DefaultValuePoints;}}
  public int pointsEarned {get { return m_PointsEarned;}}

  private bool already_enough = false;

  public delegate void ChangeStateDelegate();
  public event ChangeStateDelegate ChangeStateEvent;

	// Use this for initialization
	void Start () {
		timer = 0.0f;
    if (isServer && m_PickupManager != null) m_PickupManager.PrizePickedCallback += PrizePicked;
    if (isLocalPlayer && m_PointsCanvas != null && m_PointsCanvas.activeSelf) {
      m_PointsMessage = m_PointsCanvas.GetComponent<Text>();
      m_PointsMessage.enabled = false;
      UpdateScoreTracker();
    }
  }

  void OnDisable() {
    if (isServer && m_PickupManager != null) m_PickupManager.PrizePickedCallback += PrizePicked;
  }

  [Server]
  void PrizePicked(GameObject target) {
    if (target == null) return;
    ObjectValue m_ObjectValueScript = target.GetComponent<ObjectValue>();
    if ( m_ObjectValueScript == null) m_PointsEarned += defaultValuePoints;
    else m_PointsEarned += m_ObjectValueScript.objectValue;
    if ( !already_enough && m_PointsEarned >= necessaryPoints ) {
      if (ChangeStateEvent != null) {
        ChangeStateEvent();
        already_enough = true;
      }
    }
  }

  [ClientCallback]
  void OnPointsEarnedUpdate(int newValue) {
    m_PointsEarned = newValue;
    if ( isLocalPlayer ) ShowUpdatedCanvas();
  }

	// Update is called once per frame
	void Update () {

		if (m_PointsMessage != null && m_PointsMessage.enabled == true) {

			timer += Time.deltaTime;

			// After 3 seconds, hide text //
			if((int) timer % 60 == 3){
				m_PointsMessage.enabled = false;
				timer = 0.0f;
			}

		}
			
	}

  void ShowUpdatedCanvas() {
    if ( isLocalPlayer && m_PointsCanvas != null && m_PointsCanvas.activeSelf && m_PointsMessage != null) {
      // Update points on UI here
			//m_PointsCanvas.GetComponent<Text>();

			// update score in bottom right corner
      UpdateScoreTracker();
      ShowMessage();
    }
  }

  void UpdateScoreTracker() {
    points.text = m_PointsEarned.ToString ();
  }

  void ShowMessage() {
      timer = 0.0f;

      // display message (middle)
      m_PointsMessage.text = "You now have " + m_PointsEarned.ToString () + " points!";
      m_PointsMessage.enabled = true;
  }
}
