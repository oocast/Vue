using UnityEngine;
using System.Collections;

public class ObjectRotation : MonoBehaviour {
    public float angularSpeed;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(Vector3.up * angularSpeed * Time.deltaTime, Space.World);
	}
}
