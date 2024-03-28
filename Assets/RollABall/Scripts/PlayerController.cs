using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float jumpStrength;
	public Text countText;
	public Text winText;
	public Transform cam;
	
	public AudioClip[] clips;
	private AudioSource audioSource;

	private Rigidbody rb;
	private Vector3 respawnPoint;
	private int count;
	private bool spaceHeld;
	private bool onGround;
	private float moveHorizontal;
	private float moveVertical;

	void Start() {
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		respawnPoint = rb.position;
		count = 0;
		SetCountText ();
		winText.text = "";
	}

    void Update() {
		spaceHeld = Input.GetKey(KeyCode.Space);
		moveHorizontal = Input.GetAxis("Horizontal");
		moveVertical = Input.GetAxis("Vertical");
	}

    void FixedUpdate() {
		// JUMPING
		Vector3 pos = this.transform.position;
		Vector3 center = new Vector3(pos.x, pos.y-0.1f, pos.z);
		Collider[] objs = Physics.OverlapSphere(center, 0.45f);
		onGround = objs.Length > 1;
		if(spaceHeld && onGround) {
			rb.AddForce(new Vector3(0.0f, jumpStrength, 0.0f));
        }
		// RESPAWNING
		if(rb.position.y < 0.0f) {
			rb.position = respawnPoint;
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
        }
		// MOVING
		Quaternion cameraAngle = Quaternion.Euler(0, cam.eulerAngles.y, 0);
		Vector3 direction = new Vector3(moveHorizontal*speed, 0.0f, moveVertical*speed);
		rb.AddForce(cameraAngle * direction);
	}
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Pick Up")){
			other.gameObject.SetActive(false);
			count = count + 1;
			SetCountText();
			audioSource.PlayOneShot(clips[0]);
		}
	}

	void SetCountText() {
		countText.text = "Count: " + count.ToString ();
		if (count >= 12)  {
			winText.text = "You Win!";
			audioSource.PlayOneShot(clips[1]);
		}
	}
}