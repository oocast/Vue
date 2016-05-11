using UnityEngine;
using System.Collections;

public class CheatTeleport : MonoBehaviour {
    public Transform dropPoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.T) && dropPoint != null)
        {
            transform.position = dropPoint.position;
        }
	}
}
