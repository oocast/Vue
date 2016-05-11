using UnityEngine;
using System.Collections;

public class ElevatorBehavior : MonoBehaviour {
	public Transform target;
	public float speed;
	public bool startsRunning;
	private bool running = false;
	// Use this for initialization
	void Start () {
		if(startsRunning){
			running = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (running) {
			var step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, target.position, step);
		}


	}

	public void ToggleElevator() {
		running = !running; 
	}
}
