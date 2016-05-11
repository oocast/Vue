using UnityEngine;
using System.Collections;

public class BossTrigger : MonoBehaviour {
    public BossMovement bossMovement;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            bossMovement.enabled = true;
        }
    }
}
