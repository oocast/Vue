using UnityEngine;
using System.Collections;
using DG.Tweening;
public class ElevatorTrigger : MonoBehaviour {
	public GameObject elevator;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			elevator.GetComponent<ElevatorBehavior> ().ToggleElevator ();
		}
		Destroy (gameObject);
	}
}
