using UnityEngine;
using System.Collections;

public class SkillVote : GameContentVote {
    SkillLibrary skillLibrary;
    CharacterAttack characterAttack;

	// Use this for initialization
	void Start () {
        GameObject skillLibraryObj = GameObject.Find("Skill Library 2");
        if (skillLibraryObj != null)
        {
            skillLibrary = skillLibraryObj.GetComponent<SkillLibraryBehavior>().skillLibrary;
        }

        GameObject playerCharacter = GameObject.FindGameObjectWithTag("Player");
        characterAttack = playerCharacter.GetComponent<CharacterAttack>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected override void ChangeContent(Vote finishedCurrentVote)
    {
        int[] skillIndices = finishedCurrentVote.GetResultOptionIndices(2);
        characterAttack.UpdateWeaponAbility(skillIndices[0], skillIndices[1]);
    }
}
