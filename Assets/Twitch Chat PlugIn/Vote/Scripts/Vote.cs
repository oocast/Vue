using UnityEngine;
using System.Linq;
using System.Collections;

[System.Serializable]
public class Vote
{
    public string title;
    public string description;
    public float duration;

    /// <summary>
    /// Options should be identical across all votes
    /// </summary>
    public string[] options;
    public int resultSize = 1;

    /// <summary>
    /// Optional: the indices of the option names in other lookup tables
    /// </summary>
    public int[] optionIndices;

    protected int[] voteCounts;
    int _totalCounts;

    bool _updated;

    [HideInInspector]
    public bool open;

    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="vote"></param>
    public Vote(Vote vote)
    {
        title = vote.title;
        description = vote.description;
        duration = vote.duration;
        options = vote.options;
        optionIndices = vote.optionIndices;
    }

    /// <summary>
    /// (Re)open the vote, clear the metadata
    /// </summary>
    public void OpenVote()
    {
        open = true;
        voteCounts = new int[options.Length];
        for (int i = 0; i < voteCounts.Length; i++)
        {
            voteCounts[i] = 0;
        }
        _totalCounts = 0;
    }

    virtual public void PrepareVote(string[] parameters)
    {
    }

    virtual public string CloseVoteAndGetResult()
    {
        // TODO: break tie by time
        open = false;
        int maxCount = voteCounts.Max();
        int resultIndex = 0;
        if (maxCount > 0)
        {
            resultIndex = voteCounts.ToList().IndexOf(maxCount);
        }
        else
        {
            Debug.Log("CloseVoteAndGetResult: Random");
            resultIndex = Random.Range(0, options.Length);
        }
        return options[resultIndex];
    }

    public string GetResult()
    {
        int maxCount = voteCounts.Max();
        int resultIndex = 0;
        if (maxCount > 0)
        {
            resultIndex = voteCounts.ToList().IndexOf(maxCount);
        }
        else
        {
            Debug.Log("GetResult: Random");
            resultIndex = Random.Range(0, options.Length);
        }
        return options[resultIndex];
    }

    public string[] GetResults()
    {
        string[] result = null;
        if (options.Length >= resultSize)
        {
            int maxCount = voteCounts.Max();
            int[] resultIndices;
            if (maxCount > 0)
            {
                resultIndices = voteCounts
                    .Select((count, index) => new { count, index })
                    .OrderByDescending(item => item.count)
                    .Take(resultSize)
                    .Select(item => item.index)
                    .ToArray();
                Debug.Log(resultIndices[0]);
            }
            else
            {
                Debug.Log("GetResultIndices: Random");
                resultIndices = new int[resultSize];
                // TODO: match with result of GetResults (offline:random case)
                for (int i = 0; i < resultSize; i++)
                {
                    resultIndices[i] = -1;
                    int randomIndex = Random.Range(0, options.Length);
                    while (!resultIndices.Contains(randomIndex))
                    {
                        resultIndices[i] = randomIndex;
                    }
                }
            }

            result = new string[resultSize];
            for (int i = 0; i < resultSize; i++)
            {
                result[i] = options[resultIndices[i]];
            }
        }
        return result;
    }

    public string[] GetResults(int topSize)
    {
        string[] result = null;
        if (options.Length >= topSize)
        {
            int maxCount = voteCounts.Max();
            int[] resultIndices;
            if (maxCount > 0)
            {
                resultIndices = voteCounts
                    .Select((count, index) => new { count, index })
                    .OrderByDescending(item => item.count)
                    .Take(topSize)
                    .Select(item => item.index)
                    .ToArray();
                Debug.Log(resultIndices[0]);
            }
            else
            {
                Debug.Log("GetResultIndices: Random");
                resultIndices = new int[topSize];
                // TODO: match with result of GetResults (offline:random case)
                for (int i = 0; i < topSize; i++)
                {
                    resultIndices[i] = -1;
                    int randomIndex = Random.Range(0, options.Length);
                    while (!resultIndices.Contains(randomIndex))
                    {
                        resultIndices[i] = randomIndex;
                    }
                }
            }

            result = new string[topSize];
            for (int i = 0; i < topSize; i++)
            {
                result[i] = options[resultIndices[i]];
            }
        }
        return result;
    }

    /// <summary>
    /// Get the indices of corresponding object(skill/weapon) in library
    /// </summary>
    /// <param name="topSize"></param>
    /// <returns></returns>
    public int[] GetResultOptionIndices(int topSize)
    {
        int[] result = null;
        if (options.Length >= topSize)
        {
            int maxCount = voteCounts.Max();
            int[] resultIndices;
            if (maxCount > 0)
            {
                resultIndices = voteCounts
                    .Select((count, index) => new { count, index })
                    .OrderByDescending(item => item.count)
                    .Take(topSize)
                    .Select(item => item.index)
                    .ToArray();
            }
            else
            {
                Debug.Log("GetResultIndices: Random");
                resultIndices = new int[topSize];
                // TODO: match with result of GetResults (offline:random case)
                for (int i = 0; i < topSize; i++)
                {
                    resultIndices[i] = -1;
                    int randomIndex = Random.Range(0, options.Length);
                    while (resultIndices.Contains(randomIndex))
                    {
                        randomIndex = Random.Range(0, options.Length);
                    }
                    resultIndices[i] = randomIndex;
                }
            }

            result = new int[topSize];
            for (int i = 0; i < topSize; i++)
            {
                result[i] = optionIndices[resultIndices[i]];
            }
        }
        return result;
    }

    public int[] GetResultIndices(int topSize)
    {
        int[] result = null; 
        if (options.Length >= topSize)
        {
            int maxCount = voteCounts.Max();
            int[] resultIndices;
            if (maxCount > 0)
            {
                resultIndices = voteCounts
                    .Select((count, index) => new { count, index })
                    .OrderByDescending(item => item.count)
                    .Take(topSize)
                    .Select(item => item.index)
                    .ToArray();
                Debug.Log(resultIndices[0]);
            }
            else
            {
                Debug.Log("GetResultIndices: Random");
                resultIndices = new int[topSize];
                // TODO: match with result of GetResults (offline:random case)
                for (int i = 0; i < topSize; i++)
                {
                    resultIndices[i] = -1;
                    int randomIndex = Random.Range(0, options.Length);
                    while (resultIndices.Contains(randomIndex))
                    {
                        randomIndex = Random.Range(0, options.Length);
                    }
                    resultIndices[i] = randomIndex;
                }
            }

            result = new int[topSize];
            for (int i = 0; i < topSize; i++)
            {
                result[i] = resultIndices[i];
            }
        }
        return result;
    }

    public void IncreaseVoteCount(int index)
    {
        voteCounts[index]++;
        _totalCounts++;
    }

    public int GetVoteCount(int index)
    {
        return voteCounts[index];
    }

    public int[] GetAllCounts()
    {
        int[] result = new int[voteCounts.Length + 1];
        System.Array.Copy(voteCounts, 0, result, 1, voteCounts.Length);
        result[0] = _totalCounts;
        return result;
    }

    virtual public string GetStartAnnouncement(string optionCode)
    {
        string result = "";
        result += "Vote "+title+" begins. ";
        result += "Vote by typing \""+optionCode+"option\". ";
        result += "Options: ";
        for (int i = 0; i < options.Length; i++)
        {
			result += options[i].ToLower();
            if (i < options.Length - 1)
            {
                result += ", ";
            }
            else
            {
                result += ". ";
            }
        }
        if (options.Length < 1)
            result = null;
        return result;
    }

    virtual public string GetInstruction()
    {
        return "INPUT THE TEXT BENEATH THE ICONS BELOW TO VOTE";
    }

    virtual public bool VoterValid(string voter)
    {
        return true;
    }

    public string GetEndAnnouncement()
    {
        string result = "";
        result += "Vote " + title + " ends. ";
        if (options.Length < 1)
            result = null;
        return result;
    }

    public int GetOptionIndexByName(string optionName)
    {
        int result = -1;
        if (options.Length != optionIndices.Length)
        {
            return result;
        }

        for (int i = 0; i < options.Length; i++)
        {
            if (options[i].Equals(optionName))
            {
                result = optionIndices[i];
                break;
            }
        }
        return result;
    }

}
