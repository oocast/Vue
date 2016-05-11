using UnityEngine;
using System.Collections;

public class Trail : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetTrailActive(bool active)
    {
        GetComponent<TrailRenderer>().enabled = active;
    }
}
