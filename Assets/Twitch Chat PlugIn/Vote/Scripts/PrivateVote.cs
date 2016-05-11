using UnityEngine;
using System.Linq;
using System.Collections;

[System.Serializable]
public class PrivateVote : Vote {
    /// <summary>
    /// The font size of voter in instruction
    /// </summary>
    public int instructionVoterSize;

    /// <summary>
    /// The font size of the instruction body
    /// </summary>
    public int instructionSize;

    string[] _privilegedViewerNames;

    /// <summary>
    /// Create PrivateVote instance from a Vote instance
    /// </summary>
    /// <param name="vote"></param>
    public PrivateVote(Vote vote) : base(vote)
    {
        
    }

    /// <summary>
    /// Pick the voters
    /// </summary>
    /// <param name="privilegedViewerNames">List of chosen twitch users in channel</param>
    public void AssignPrivilegedViewerNames(string[] privilegedViewerNames)
    {
        this._privilegedViewerNames = privilegedViewerNames;
    }

    /// <summary>
    /// Create vote start announcement to chat window. 
    /// </summary>
    /// <param name="optionCode">The prefix denoting a vote</param>
    /// <returns>The vote start announcement used in chat window</returns>
    override public string GetStartAnnouncement(string optionCode)
    {
        string result = "";
        result += "Vote " + title + " begins. ";
        result += "Vote by typing \"" + optionCode + "<option>\". ";
        for (int i = 0; i < _privilegedViewerNames.Length; i++)
        {
            result += (_privilegedViewerNames[i] + ", ");
        }
        result += "you are picked up for this vote! ";
        result += description + ". ";
        result += "Options: ";
        for (int i = 0; i < options.Length; i++)
        {
            result += options[i];
            if (i < options.Length - 1)
            {
                result += ", ";
            }
            else
            {
                result += ". ";
            }
        }
        return result;
    }

    /// <summary>
    /// Create vote instruction to chat UI. Rich text syntax used to change size.
    /// </summary>
    /// <returns>The vote instruction</returns>
    override public string GetInstruction()
    {
        string suffix = "INPUT THE TEXT BENEATH THE ICONS BELOW TO VOTE";
        string voterSizeTag = string.Format("<size={0}>", instructionVoterSize);
        string instructionSizeTag = string.Format("<size={0}>", instructionSize);
        string result = "";
        for (int i = 0; i < _privilegedViewerNames.Length; i++)
        {
            result += voterSizeTag + "<i>" + _privilegedViewerNames[0] + "</i></size>, ";
        }
        result += instructionSizeTag + suffix + "</size>";
        return result;
    }


    override public string CloseVoteAndGetResult()
    {
        // TODO: break tie by voting time
        open = false;
        int maxCount = voteCounts.Max();
        int resultIndex = 0;
        if (maxCount > 0)
        {
            resultIndex = voteCounts.ToList().IndexOf(maxCount);
            return options[resultIndex];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Check whether the voter is valid for a privileged voter
    /// </summary>
    /// <param name="voter"></param>
    /// <returns></returns>
    public override bool VoterValid(string voter)
    {
        return _privilegedViewerNames.Contains(voter);
    }

    /// <summary>
    /// Prepare the vote with an array of privileged twitch user ids
    /// </summary>
    /// <param name="parameters">An array of privileged twitch user ids</param>
    override public void PrepareVote(string[] parameters)
    {
        AssignPrivilegedViewerNames(parameters);
    }
}
