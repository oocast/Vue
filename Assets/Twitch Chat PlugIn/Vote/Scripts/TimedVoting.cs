using UnityEngine;
using System.Collections;
using System.Linq;

public class TimedVoting : MonoBehaviour {
    /// <summary>
    /// Interface to deliver vote information for game content change from vote
    /// </summary>
    public event System.Action<Vote[]> onVoteSwitch;
    public event System.Action<Vote[]> onVoteClose;

    /// <summary>
    /// Array of vote to random through
    /// </summary>
    public Vote[] votes;
    
    /// <summary>
    /// Indices of votes not in the random system
    /// </summary>
    public int[] exemptionIndices;
    
    /// <summary>
    /// Whether use the first vote slot for an empty vote as break
    /// </summary>
    public bool firstVoteBreak;

    VotingSystem _votingSystem;
    public float switchTime;

    /// <summary>
    /// Used for debug and testing
    /// </summary>
    public int overrideFirstVoteIndex;

    /// <summary>
    /// Vote queue to specify the first N votes
    /// </summary>
    public int[] earlyVoteIndexQueue;

    int voteCount = 0;

    // Vote indices
    int currentVoteIndex = -1;
    int nextVoteIndex = -1;
    int nextVoteIndexOverride = -1;
    
    void Awake()
    {
        _votingSystem = GameObject.Find("Twitch Vote").GetComponent<VotingSystem>();
    }


	// Use this for initialization
	void Start () {
        voteCount = 0;
        currentVoteIndex = -1;
        nextVoteIndex = -1;
        nextVoteIndexOverride = -1;
        StartFirstVote();
    }

    /// <summary>
    /// Wait for waitTime (sec), and switch next vote to current vote
    /// </summary>
    /// <param name="waitTime">Wait time in seconds</param>
    /// <returns></returns>
    IEnumerator SwitchVote(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // handle the current vote
        Vote currentVote = null;
        Vote nextVote = null;
        if (currentVoteIndex != -1)
        {
            currentVote = votes[currentVoteIndex];
            currentVote.open = false;
        }
        
        // switch to next vote
        if (nextVoteIndex != -1)
        {
            nextVote = votes[nextVoteIndex];

            // There is a "Break" vote, and next vote exists and is not a break
            // Ensure there is a break between votes
            if (firstVoteBreak && nextVoteIndex > 0)
            {
                // Set further next vote as break
                currentVoteIndex = nextVoteIndex;
                nextVoteIndex = 0;
            }
            // There is override vote
            else if (nextVoteIndexOverride >= 0)
            {
                currentVoteIndex = nextVoteIndex;
                nextVoteIndex = nextVoteIndexOverride;
                nextVoteIndexOverride = -1;
            }
            // The early vote queue is not exhausted
            else if (voteCount < earlyVoteIndexQueue.Length)
            {
                currentVoteIndex = nextVoteIndex;
                nextVoteIndex = earlyVoteIndexQueue[voteCount];
                voteCount++;
            }
            else
            {
                int randIndex = Random.Range(0, votes.Length);
                // Make sure consecutive votes are different
                // Warning: make sure there are at least two unexempted votes
                while (randIndex == nextVoteIndex || randIndex == currentVoteIndex || exemptionIndices.Contains(randIndex))
                {
                    randIndex = Random.Range(0, votes.Length);
                }
                currentVoteIndex = nextVoteIndex;
                nextVoteIndex = randIndex;
            }

            StartCoroutine(SwitchVote(nextVote.duration + switchTime));
            _votingSystem.RegisterNewVote(nextVote);
        }

        if (onVoteSwitch != null)
        {
            Debug.Log("onVoteSwitch not null");
            onVoteSwitch(new Vote[] { currentVote, nextVote, votes[nextVoteIndex] });
        }
    }

    /// <summary>
    /// Deprecated
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator CloseVote(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Vote currentVote = null;
        if (currentVoteIndex != -1)
        {
            currentVote = votes[currentVoteIndex];
            currentVote.open = false;
        }
        Vote nextVote = null;
        if (nextVoteIndex != -1)
        {
            nextVote = votes[nextVoteIndex];
        }

        if (onVoteClose != null)
        {
            onVoteClose(new Vote[] { currentVote, nextVote });
        }
    }

    void StartFirstVote()
    {
        
        if (votes.Length > 1)
        {
            int startIndex = firstVoteBreak ? 1 : 0;
            int randIndex = Random.Range(startIndex, votes.Length);
            while (exemptionIndices.Length > 0 && exemptionIndices.Contains(randIndex))
            {
                randIndex = Random.Range(startIndex, votes.Length);
            }
            nextVoteIndex = randIndex;
            if (earlyVoteIndexQueue.Length > 0)
            {
                nextVoteIndex = earlyVoteIndexQueue[0];
            }

            // StartCoroutine(CloseVote(0f));
            StartCoroutine(SwitchVote(switchTime));
            voteCount++;
        }
    }

    /// <summary>
    /// Override the next vote for next switching
    /// </summary>
    /// <param name="index"></param>
    public void OverrideNextIndex(int index)
    {
        nextVoteIndexOverride = index;
    }

    public void OverrideVote (Vote vote, int index)
    {
        if (index < votes.Length && index > -1)
        {
            votes[index] = vote;
        }
    }

    /// <summary>
    /// Return the vote object if title match, otherwise null
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    public Vote GetVoteByTitle(string title)
    {
        Vote result = null;
        foreach (Vote vote in votes)
        {
            if (vote.title.Equals(title))
            {
                result = vote;
                break;
            }
        }
        return result;
    }

    /// <summary>
    /// Return the index of vote object if title match, otherwise -1
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    public int GetIndexByTitle(string title)
    {
        int result = -1;
        for (int i = 0; i < votes.Length; i++)
        {
            if (votes[i].title.Equals(title))
            {
                result = i;
                break;
            }
        }
        return result;
    }
}
