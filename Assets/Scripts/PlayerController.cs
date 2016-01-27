using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {


    //[SerializeField] private Camera m_Camera;
    //[SerializeField] private SoundVision m_SoundsVision;
    [SerializeField] private bool is_Walking;
    [SerializeField] private float m_Speed = 2.0f;
    private Vector3 m_MoveDirection = Vector3.zero;
    private Vector2 m_Input;

    public Camera cam;
	public SoundVision soundVision;
    public AudioSource audio_source;

    bool turned;
	// public SoundVision test;
    // Use this for initialization
    void Start () {
	    turned = false;

      // soundVision = GetComponent<SoundVision>();
        audio_source = GetComponent<AudioSource>();
        audio_source.clip = (AudioClip)Resources.Load("AudioClips/Footstep1");
    }
	
	// Update is called once per frame
	void Update () {
        
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 moveDirection = transform.forward*moveVertical + transform.right*moveHorizontal;
        if (moveVertical == 1.0)
        {
            transform.position = transform.position + cam.transform.forward * 2 * Time.deltaTime;
            if (audio_source.isPlaying == false)// add more logic later such as, onground/jumping etc etc
            {
                //AudioSource.PlayClipAtPoint(footstep_Sound1, transform.position);
                //footstep_playing = 1;
                audio_source.pitch = Random.Range(0.8f, 1);
                audio_source.volume = Random.Range(0.8f, 1.1f);
                audio_source.Play();
                soundVision.CreateSound(transform.position, 15);
            }
        }

        if ((moveHorizontal == 1.0 || moveHorizontal == -1.0) && turned == false)
        {
            transform.Rotate(new Vector3(0.0f, moveHorizontal * 30, 0.0f));
            turned = true;
        } else if (moveHorizontal != 1.0 && moveHorizontal != -1.0)
        {
            turned = false;
        }
    }

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Explorer") {
			Debug.Log("MUMMY WINS");
		}
	}
}
