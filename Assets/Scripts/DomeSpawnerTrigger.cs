using UnityEngine;
using System.Collections;

public class DomeSpawnerTrigger : MonoBehaviour {

	public GameObject[] domespawners;
	private bool hasBeenTriggered;
	// Use this for initialization
	void Start () {
		hasBeenTriggered = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player" && !hasBeenTriggered){
			foreach (GameObject domeSpanwer in domespawners) {
				domeSpanwer.SetActive (true);
			}
			hasBeenTriggered = true;
		}
	}
}
