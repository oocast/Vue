using UnityEngine;
using System.Collections;

public class DoorBehavior : MonoBehaviour {
	bool isOpen = true;
	public float openTranslation;
	public bool startsOpen;
	// Use this for initialization
	void Start () {
		if (startsOpen) {
			OpenDoor ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Opens the door
	public void OpenDoor() {
		isOpen = true;
		transform.Translate (Vector3.up * openTranslation);
	}

	// Closes door
	public void CloseDoor() {
		isOpen = false;
		transform.Translate (Vector3.down * openTranslation);
	}
}
