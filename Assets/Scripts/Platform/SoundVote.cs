using UnityEngine;
using System.Collections;

public class SoundVote : GameContentVote {

    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected override void ChangeContent(params string[] content)
    {
        Debug.Log("Sound vote change content");
    }
}
