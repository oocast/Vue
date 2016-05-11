using UnityEngine;
using System.Collections;

[System.Serializable]
public class Skill {
    public string skillName;
    public int skillID;

    public string colorTag;

    // TODO: change to ability slot
    public string iconName;

    /// <summary>
    /// The time between button click and the skill *active*
    /// </summary>
    public float castTime;

    /// <summary>
    /// The time between skill activation and end
    /// </summary>
    public float duration;

    /// <summary>
    /// The time between skill finish and unlock
    /// </summary>
    public float coolDown;

    /// <summary>
    /// Period should be less than duration. If period = 0, it is one shot effect
    /// </summary>
    public float period;

    public int[] damageList;

    /// <summary>
    /// Whether the caster can use other skills in the <duration> 
    /// </summary>
    public bool disableSkill;

    /// <summary>
    /// Whether the caster can move in the <duration>
    /// </summary>
    public bool disableMove;

    /// <summary>
    /// "Enemy" or "Player"
    /// </summary>
    public string targetTag;

    #region effects
    /// <summary>
    /// Amount of time stunning the targets
    /// </summary>
    public float stunTime;

    /// <summary>
    /// Distance of charging, teleport, nockback
    /// </summary>
    public float moveDistance;
    #endregion

    public Skill()
    {
        skillName = "Skill Name";
        targetTag = "Enemy";
        iconName = "Icon Name";
        damageList = new int[2];
    }

    virtual public GameObject[] GetTargets()
    {
        Debug.LogWarning("GetTargets is not implemented");
        return null;
    }

    virtual public GameObject[] GetTargets(Transform referenceTransform)
    {
        Debug.LogWarning("GetTargets is not implemented");
        return null;
    }

    virtual public void ActivateSkill(GameObject caster)
    {
        GameObject.FindGameObjectWithTag("SkillManager").GetComponent<SkillManager>().ActivateSkill(this, GetReferenceTransform(caster), caster);
    }

    /// <summary>
    /// Cause damage/heal, make effect, shoot shock wave, etc.
    /// </summary>
    virtual public void UpdateEffect(Transform referenceTransform = null, GameObject skillCaster = null)
    {
        Debug.LogWarning("UpdateEffect is not implemented");
    }

    /// <summary>
    /// Clear locks
    /// </summary>
    virtual public void OnSkillEnd()
    {
        Debug.LogWarning("OnSkillEnd is not implemented");
    }

    /// <summary>
    /// Return referenceTransform
    /// </summary>
    /// <returns>
    /// The self transform or create a new one
    /// </returns>
    virtual public Transform GetReferenceTransform(GameObject skillCaster)
    {
        return skillCaster.transform;
    }

    virtual public void OnActivate(GameObject skillCaster)
    {
        SoundSystem sound = GameObject.Find("Sound System").GetComponent<SoundSystem>();
        if (sound != null)
        {
            sound.PlaySound(Camera.main.transform.position, iconName);
        }

        // TODO: implement with move, attack lock
        if (skillCaster == GameObject.FindGameObjectWithTag("Player"))
        {
            if (disableSkill == true)
            {
                skillCaster.GetComponent<CharacterAttack>().LockAttack();
            }
        }
    }

    virtual public void OnDeactivate(GameObject skillCaster)
    {
        // TODO: implement with move, attack lock
        if (skillCaster == GameObject.FindGameObjectWithTag("Player"))
        {
            if (disableSkill == true)
            {
                skillCaster.GetComponent<CharacterAttack>().UnlockAttack();
            }
        }
    }
}
