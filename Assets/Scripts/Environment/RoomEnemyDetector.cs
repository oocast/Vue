using UnityEngine;
using System.Collections;

public class RoomEnemyDetector : MonoBehaviour {
	public GameObject[] doors;
	private bool roomFinished;
	private bool encounterStarted;
	Collider[] enemies;

	// Use this for initialization
	void Start () {
		roomFinished = false;
		encounterStarted = false;
	}
	
	// Update is called once per frame
	void Update () {
		//enemies = Physics.OverlapBox (gameObject.transform.position, transform.lossyScale, Quaternion.identity,LayerMask.GetMask("Enemy"));
		enemies = Physics.OverlapSphere(gameObject.transform.position, GetComponent<SphereCollider>().radius * transform.lossyScale.x,256);


		if (enemies.Length == 0 && !roomFinished && encounterStarted) {
			foreach (GameObject door in doors) {
				door.GetComponent<DoorBehavior> ().OpenDoor ();
			}
			roomFinished = true;
		}
			

	}

	public void StartEncounter() {
		encounterStarted = true;
		foreach (GameObject door in doors) {
			var doorBehavior = door.GetComponent<DoorBehavior> ();
			// Closes the door behind them
			if (doorBehavior.startsOpen) {
				doorBehavior.CloseDoor ();
			} else {
				// Releases caged enemies
				doorBehavior.OpenDoor ();
			}

		}
	}




}
