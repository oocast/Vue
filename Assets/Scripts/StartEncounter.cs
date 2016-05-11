using UnityEngine;
using System.Collections;

public class StartEncounter : MonoBehaviour {

	public RoomEnemyDetector[] enemyDetectors;
	private bool hasBeenTriggeredByPlayer;
	// Use this for initialization
	void Start () {
		hasBeenTriggeredByPlayer = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && !hasBeenTriggeredByPlayer) {
			foreach(RoomEnemyDetector enemyDetector in enemyDetectors){
				enemyDetector.GetComponent<RoomEnemyDetector> ().StartEncounter ();
			}
			hasBeenTriggeredByPlayer = true;
		}

	}
}
