using UnityEngine;
using System.Collections;

[System.Serializable]
public class SkillManagerEntry {
    /// <summary>
    /// Center and direction of Skill
    /// </summary>
    public Transform referenceTransform;

    /// <summary>
    /// The character uses this skill
    /// </summary>
    public GameObject skillCaster;

    /// <summary>
    /// Start time + duration
    /// </summary>
    public float activeEndTime;

    /// <summary>
    /// Start time + castTime, update by the period
    /// </summary>
    public float nextUpdateTime;

    /// <summary>
    /// If suspended = true, no more active effect, only passive effect
    /// </summary>
    public bool suspended;

    /// <summary>
    /// If interrupted = true, 
    /// </summary>
    public bool interrupted;

    public Skill skill;

    public SkillManagerEntry(Skill skill, Transform referenceTransform, GameObject skillCaster)
    {
        this.referenceTransform = referenceTransform;
        this.skillCaster = skillCaster;
        this.skill = skill;
        activeEndTime = Time.time + skill.duration;
        nextUpdateTime = Time.time + skill.castTime;
        suspended = false;
        interrupted = false;
    }

    public void UpdateEffect()
    {
        skill.UpdateEffect(referenceTransform, skillCaster);
    }
}
