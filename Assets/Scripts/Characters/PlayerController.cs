using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public GameObject ground;
	Rigidbody rgbd;
	
	public int startingHealth = 100;                            // The amount of health the player starts the game with.
	public int currentHealth;
	bool isDead;                                                // Whether the player is dead.
	
	// Use this for initialization
	void Start () {
		rgbd = GetComponent <Rigidbody > ();
	}
	
	// Update is called once per frame
	void Update () {
		//PC input
		/*float MoveHor = Input.GetAxis ("Horizontal");
		float MoveVer = Input.GetAxis ("Vertical");
		Vector3 movement = new Vector3 (MoveHor, 0, MoveVer);
		rgbd .AddForce (movement * speed);
		*/
		
		//touch input
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
		{
			Vector2 touchPosition = Input.GetTouch(0).position;
			double halfScreen = Screen.width / 2.0;
			//Check if it is left or right?
			if (touchPosition.x < halfScreen)
			{
				this.transform.Translate(Vector3.left * 5 * Time.deltaTime);
			}
			else if (touchPosition.x > halfScreen)
			{
				this.transform.Translate(Vector3.right * 5 * Time.deltaTime);
			}
		}
		
	}
}
