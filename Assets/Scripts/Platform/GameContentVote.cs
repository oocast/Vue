using UnityEngine;
using System.Collections;

public class GameContentVote : MonoBehaviour {
    Vote vote;
    public string targetVoteTitle;

    virtual protected void Awake()
    {
        TimedVoting timedVoting = GameObject.Find("Twitch Vote").GetComponent<TimedVoting>();
        if (timedVoting != null)
        {
            timedVoting.onVoteSwitch += CheckVote;
        }
        Debug.Log("Enque vote " + targetVoteTitle);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void CheckVote(Vote[] votes)
    {
        if (votes.Length > 0)
        {
            vote = votes[0];
            if (vote != null && vote.title.Equals(targetVoteTitle))
            {
                string result = vote.GetResult();
                ChangeContent(result);
                ChangeContent(vote);
            }
        }
    }

    protected virtual void ChangeContent(params string[] content)
    {

    }

    protected virtual void ChangeContent(Vote finishedCurrentVote)
    {

    }
}
