using UnityEngine;
using System.Collections;

public class PlayerInventory : MonoBehaviour {
    [HideInInspector]
    public bool holdChestKey;
    public ChestKey chestKey;

	// Use this for initialization
	void Start () {
        holdChestKey = false;
        chestKey = null;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GrabKey(ChestKey key)
    {
        holdChestKey = true;
        chestKey = key;
    }

    public void UseKey()
    {
        holdChestKey = false;
        chestKey = null;
    }
}
