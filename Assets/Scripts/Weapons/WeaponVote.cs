using UnityEngine;
using System.Collections;
using DG.Tweening;

public class WeaponVote : MonoBehaviour {
    Vote vote;


    Transform _playerTransform;
    WeaponLibrary _weaponLibrary;

    void Awake()
    {
        _weaponLibrary = GameObject.Find("Weapon Library").GetComponent<WeaponLibraryBehavior>().weaponLibrary;
        TimedVoting timedVoting = GameObject.Find("Twitch Vote").GetComponent<TimedVoting>();
        timedVoting.onVoteSwitch += CheckVote;
    }

    // Use this for initialization
    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update () {
	
	}

    void SpawnWeapon(string optionName)
    {
        int weaponIndex = vote.GetOptionIndexByName(optionName);
        Weapon weapon = _weaponLibrary.weapons[weaponIndex];
        _playerTransform.GetComponent<CharacterAttack>().EquipWeapon(weapon);
        _playerTransform.GetComponentInChildren<PlayerAnimation>().SetWeaponTrail(false);
    }

    void CheckVote(Vote[] votes)
    {
        if (votes.Length > 0)
        {
            vote = votes[0];
            if (vote != null && vote.title.Equals("WeaponVote"))
            {
                string result = vote.GetResult();
                SpawnWeapon(result);
            }
        }
    }
}
