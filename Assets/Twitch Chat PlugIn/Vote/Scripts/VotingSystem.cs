using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class VotingSystem : MonoBehaviour {
    public event System.Action<int[]> onCountChange;

    /// <summary>
    /// The prefix noting the chat text is voting
    /// Can be #, !, <empty>, etc..
    /// </summary>
    public string votePrefix;

    /// <summary>
    /// Whether to send announcement to chat window
    /// </summary>
    public bool chatAnnouncement;

    /// <summary>
    /// Array of votes
    /// Make sure all the options are globally identical
    /// </summary>
    List<Vote> _votes = new List<Vote>();

    /// <summary>
    /// Record making sure one viewer's vote is only count once
    /// </summary>
    Dictionary<string, int> _voterRecord;

    /// <summary>
    /// The time the voter votes last time
    /// </summary>
    Dictionary<string, float> _voterTime;

    /// <summary>
    /// The total number of active votes at same time
    /// </summary>
    static readonly int _voteListCapacity = 16;

    ChatBot _chatBot;

    [HideInInspector]
    public bool update = false;
    int _updateVoteIndex = -1;

    void Awake()
    {
        _chatBot = GameObject.Find("Twitch Chat Bot").GetComponent<ChatBot>();
        _chatBot.onChatUserMessageParse += GetViewerVote;
    }

    // Use this for initialization
    void Start ()
    {
        _votes.Capacity = _voteListCapacity;
        _voterRecord = new Dictionary<string, int>();
        _voterTime = new Dictionary<string, float>();
        update = false;
    }
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetKeyDown(KeyCode.V))
        {
            // Test if deleting initial attaching object will cause null reference
            Debug.Log(_votes[0].open);
        }
	}

    /// <summary>
    /// Add vote to vote system and activate
    /// </summary>
    /// <param name="inputVote"> </param>
    public void RegisterNewVote(Vote inputVote)
    {
        int entryIndex = _votes.Count;
        // Same vote to reuse the slot
        if (_votes.Contains(inputVote))
        {
            entryIndex = _votes.IndexOf(inputVote);
            if (_votes[entryIndex].open)
            {
                Debug.Log("Vote already registered");
                return;
            }
        }
        // Vote not in the slot
        else
        {
            // Slot used up
            if (entryIndex == _voteListCapacity)
            {  
                // Clear one vote from 
                for (int i = 0; i < _voteListCapacity; i++)
                {
                    // Find a idle vote slot  
                    if (!_votes[i].open)
                    {
                        entryIndex = i;
                        break;
                    }
                }
                
                // Valid slot not found
                if (entryIndex == _voteListCapacity)
                {
                    Debug.Log("vote list capacity reached");
                    return;
                }
            }
            else
            {
                // Change array size first, new index is pointing the last element
                _votes.Add(null);
            }
        }
        // Add new vote and OPEN it
        inputVote.OpenVote();
        int clearMask = ~(1 << entryIndex);
        List<string> voterNames = new List<string>(_voterRecord.Keys);
        foreach (string voterName in voterNames)
        {
            _voterRecord[voterName] &= clearMask;
        }
        _votes[entryIndex] = inputVote;

        // Make announcement on Twitch channel
        string announcement = _votes[entryIndex].GetStartAnnouncement(votePrefix);
        if (chatAnnouncement && announcement != null)
        {
            _chatBot.SendChatMessage(announcement);
        }
    }

    /// <summary>
    /// Close the vote and get the 1st ranked option name
    /// </summary>
    /// <param name="title">Title of the vote</param>
    /// <returns>The 1st ranked option name</returns>
    public string CloseVoteAndGetResult(string title)
    {
        string result = null;
        for (int i = 0; i < _votes.Count; i++)
        {
            if (_votes[i].title == title)
            {
                result = _votes[i].CloseVoteAndGetResult();
                _chatBot.SendChatMessage(title + " ends! Result is "+result + ".");
                break;
            }
        }
        return result;
    }

    /// <summary>
    /// Called from ChatBot to process the user name and chat message
    /// </summary>
    /// <param name="userName">user name of twitch user who send message</param>
    /// <param name="chatMessage">the message the user sent to chat</param>
    void GetViewerVote(string userName, string chatMessage)
    {
        // Update the voter time, used to find privileged users
        if (_voterTime.ContainsKey(userName))
        {
            _voterTime[userName] = Time.time;
        }
        else
        {
            _voterTime.Add(userName, Time.time);
        }

        // Only care about voting messages
        if (chatMessage.StartsWith(votePrefix))
        {
            string optionMessage = chatMessage.Substring(1);
            bool matchFound = false;
            // Search for all active votes
            for (int i = 0; i < _votes.Count && !matchFound; i++)
            {
                Vote vote = _votes[i];
                if (!vote.open)
                {
                    continue;
                }
                // Search for matching option name
                for (int j = 0; j < vote.options.Length; j++)
                {
                    // Loose match using lower case
                    if (optionMessage.ToLower().Equals(vote.options[j].ToLower()))
                    {
                        matchFound = true;
                        _updateVoteIndex = i;
                        int voteMask = (1 << i);
                        
                        // For each vote each viewer, only the first option voted counts
                        // The user never voted before
                        if (!_voterRecord.ContainsKey(userName) && vote.VoterValid(userName))
                        {
                            _votes[i].IncreaseVoteCount(j);
                            _voterRecord.Add(userName, voteMask);
                            update = true;
                            _chatBot.SendChatMessage(userName + " votes for " + vote.options[j]);
                            Debug.Log("User " + userName + " votes for " + vote.options[j]);

                            // Send out the counts of each option
                            if (onCountChange != null)
                            {
                                onCountChange(_votes[i].GetAllCounts());
                            }
                        }
                        // The user voted before but not voted for this
                        else if ((_voterRecord[userName] & voteMask) == 0 && vote.VoterValid(userName))
                        {
                            _votes[i].IncreaseVoteCount(j);
                            _voterRecord[userName] |= voteMask;
                            update = true;
                            _chatBot.SendChatMessage(userName + " votes for " + optionMessage);
                            Debug.Log("User " + userName + " votes for " + optionMessage);

                            // Send out the counts of each option
                            if (onCountChange != null)
                            {
                                onCountChange(_votes[i].GetAllCounts());
                            }
                        }
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Return the bare bone text showing running vote counts, only used for debug
    /// </summary>
    /// <returns>bare bone text showing running vote counts</returns>
    public string GetVoteBoardText()
    {
        string result = "";
        if (update)
        {
            update = false;
            for (int i = 0; i < _votes.Count; i++)
            {
                Vote vote = _votes[i];
                result += (vote.title + Environment.NewLine);
                for (int j = 0; j < vote.options.Length; j++)
                {
                    result += vote.options[j];
                    result += (": " + vote.GetVoteCount(j) + Environment.NewLine);
                }
                result += Environment.NewLine;
            }
        }
        return result;
    }

    /// <summary>
    /// Get a specified number of users active on chat in the last one minute
    /// </summary>
    /// <param name="size">Number of active users queried</param>
    /// <returns></returns>
    public string[] GetActiveViewerList(int size = 1)
    {
        List<string> result = new List<string>();
        int i = 0;
        float currentTime = Time.time;
        foreach (var pair in _voterTime)
        {
            // if the user votes in last minute
            if (currentTime - pair.Value < 60f)
            {
                result.Add(pair.Key);
                i++;
            }
            if (i >= size)
            {
                break;
            }
        }
        return result.ToArray();
    }
}
