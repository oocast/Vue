using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {
   // public GameObject enemyPrefab;
    public GameObject[] exists;
    int size;
    // Use this for initialization
    void Start () {
        exists = GameObject.FindGameObjectsWithTag("Enemy");
    }
	
	// Update is called once per frame
	void Update () {
        //if (exists == null)
        exists = GameObject.FindGameObjectsWithTag("Enemy");
        size = exists.Length;
        if (size == 0)
            Destroy(gameObject);
    }
}
