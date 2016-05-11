using UnityEngine;
using System.Collections;

public class ChestKey : MonoBehaviour {
    public event System.Action onPickUpKey;
    public event System.Action<GameObject> onPickUpKeyAssignChest;

    public Vote vote;
    bool registered = false;
    VotingSystem votingSystem;

    /// <summary>
    /// Corresponding chest of this key
    /// </summary>
    public GameObject chest;

	// Use this for initialization
	void Start () {
        votingSystem = GameObject.Find("Twitch Vote").GetComponent<VotingSystem>();

    }
	
	// Update is called once per frame
	void Update () {
	    if (registered == false && Input.GetKeyDown(KeyCode.M))
        {
            // votingSystem.RegisterNewVote(vote);
        }
        if (registered == false && Input.GetKeyDown(KeyCode.R))
        {
            // votingSystem.CloseVoteAndGetResult(vote.title);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (registered == false && other.tag == "Player" && !other.GetComponent<PlayerInventory>().holdChestKey)
        {
            votingSystem.RegisterNewVote(vote);
            registered = true;
            other.GetComponent<PlayerInventory>().GrabKey(this);
            Destroy(gameObject, 0.2f);
            chest.SetActive(true);
            if (onPickUpKeyAssignChest != null)
            {
                onPickUpKeyAssignChest(chest);
            }
        }
    }
}
