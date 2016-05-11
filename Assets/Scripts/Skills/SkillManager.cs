using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour {

    LinkedList<SkillManagerEntry> skillList;
	
    void Start()
    {
        skillList = new LinkedList<SkillManagerEntry>();
    }

    void Update()
    {
        UpdateSkillList();
    }

    public void ActivateSkill(Skill skill, Transform referenceTransform, GameObject skillCaster)
    {
        SkillManagerEntry entry = new SkillManagerEntry(skill, referenceTransform, skillCaster);
        skillList.AddLast(entry);
        skill.OnActivate(skillCaster);
    }

    /// <summary>
    /// Update skill effects when period reaches and remove skill when duration reaches
    /// </summary>
    void UpdateSkillList()
    {
        float currentTime = Time.time;
        var node = skillList.First;
        while (node != null)
        {
            var next = node.Next;
            SkillManagerEntry entry = node.Value;
            if (currentTime > entry.nextUpdateTime && entry.suspended == false)
            {
                entry.UpdateEffect();
                if (entry.skill.period > 0.0001f)
                {
                    // continuous skill
                    entry.nextUpdateTime += entry.skill.period;
                }
                else
                {
                    // One shot skill
                    entry.suspended = true;
                }
            }

            if (currentTime > entry.activeEndTime || entry.interrupted == true)
            {
                // time past duration
                skillList.Remove(node);
                entry.skill.OnDeactivate(entry.skillCaster);
                node = next;
                continue;
            }

            

            node = next;
        }
    }
}
