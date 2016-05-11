using UnityEngine;
using System.Collections;

public class VoteTextBoard : MonoBehaviour {
    VotingSystem votingSystem;

    void Awake()
    {
        votingSystem = GameObject.Find("Twitch Vote").GetComponent<VotingSystem>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        string newText = votingSystem.GetVoteBoardText();
        if (!newText.Equals(""))
        {
            GetComponent<UnityEngine.UI.Text>().text = newText;
        }

    }
}
