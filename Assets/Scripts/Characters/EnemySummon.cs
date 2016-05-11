using UnityEngine;
using System.Collections;

public class EnemySummon : MonoBehaviour {
    bool voteStart = false;
    string summoner;
    public PrivateVote vote;
    Vote externalVote;
    OptionDictionary optionDictionary;
    public string externalVoteTitle;
    VotingSystem votingSystem;
    TimedVoting timedVoting;
    public Transform spawnPoint;


	// Use this for initialization
	void Start () {
        votingSystem = GameObject.Find("Twitch Vote").GetComponent<VotingSystem>();
        timedVoting = GameObject.Find("Twitch Vote").GetComponent<TimedVoting>();
        if (timedVoting != null)
        {
            timedVoting.onVoteSwitch += SummonEnemy;
        }
        externalVote = null;

        GameObject optionDictionaryObj = GameObject.Find("Option Dictionary");
        if (optionDictionaryObj != null)
        {
            optionDictionary = optionDictionaryObj.GetComponent<OptionDictionary>();
        }

        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void RegisterSummonVote()
    {
        if (voteStart == true)
        {
            return;
        }
        
        string[] viewerList = votingSystem.GetActiveViewerList();
        
        if (viewerList.Length < 1)
        {
            Debug.Log("No active viewer");
            return;
        }
        else
        {
            // Add trigger lock
            voteStart = true;

            // Choose voter
            string[] pickedViewers = new string[1];
            pickedViewers[0] = viewerList[Random.Range(0, viewerList.Length)];
            summoner = pickedViewers[0];
            vote.AssignPrivilegedViewerNames(pickedViewers);

            // Open vote
            // triggerBox.enemySummon = this;
            // votingSystem.RegisterNewVote(vote);

            // Get external vote
            externalVote = timedVoting.GetVoteByTitle(externalVoteTitle);
            // externalVote.PrepareVote(pickedViewers);
            // externalVote = vote;
            int externalVoteIndex = timedVoting.GetIndexByTitle(externalVoteTitle);
            timedVoting.OverrideVote(vote, externalVoteIndex);
            timedVoting.OverrideNextIndex(externalVoteIndex);

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            RegisterSummonVote();
        }
    }

    public void SummonEnemy(Vector3 position)
    {
        string result = votingSystem.CloseVoteAndGetResult(vote.title);
        string prefabName = optionDictionary.GetPrefabResourceFileName(result);
        if (result != null)
        {
            GameObject monsterPrefab = Resources.Load("Prefabs/" + prefabName) as GameObject;
            Debug.Log(monsterPrefab == null);
            GameObject enemy = Instantiate(monsterPrefab, position, Quaternion.identity) as GameObject;
            TurretFollow follow = enemy.GetComponent<TurretFollow>();
            if (follow != null)
            {
                follow.target = GameObject.FindGameObjectWithTag("Player");
            }
            GameObject nameTagPrefab = Resources.Load("Prefabs/Name Tag Prefab") as GameObject;
            Debug.Log(nameTagPrefab == null);
            GameObject nameTag = Instantiate(nameTagPrefab) as GameObject;
            nameTag.transform.SetParent(GameObject.Find("Canvas").transform);
            nameTag.GetComponent<NameTag>().Initialize(summoner, enemy.transform);
        }
    }

    void SummonEnemy(Vote[] votes)
    {
        if (votes[0] == vote)
        {
            string result = votingSystem.CloseVoteAndGetResult(vote.title);
            string prefabName = optionDictionary.GetPrefabResourceFileName(result);
            if (result != null)
            {
                GameObject monsterPrefab = Resources.Load("Prefabs/" + prefabName) as GameObject;
                Debug.Log(monsterPrefab == null);
                GameObject enemy = Instantiate(monsterPrefab, spawnPoint.position, Quaternion.identity) as GameObject;
                TurretFollow follow = enemy.GetComponent<TurretFollow>();
                if (follow != null)
                {
                    follow.target = GameObject.FindGameObjectWithTag("Player");
                }
                GameObject nameTagPrefab = Resources.Load("Prefabs/Name Tag Prefab") as GameObject;
                Debug.Log(nameTagPrefab == null);
                GameObject nameTag = Instantiate(nameTagPrefab) as GameObject;
                nameTag.transform.SetParent(GameObject.Find("Canvas").transform);
                nameTag.transform.SetAsFirstSibling();
                nameTag.GetComponent<NameTag>().Initialize(summoner, enemy.transform);
            }
        }
    }
}
