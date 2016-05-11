using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum WeaponType
{
    SWORD,
    AXE,
    HAMMER
}

[System.Serializable]
public class Weapon : IWeapon
{
    public string weaponName;
    public int weaponId;
    public string prefabName;
    public WeaponType type;

    /// <summary>
    /// The maximum combo done with WeaponAttack()
    /// </summary>
    public int comboLengthCapacity;
    private int _comboLength;

    /// <summary>
    /// If the next basic attack is not casted before comboTime, go back to first phase
    /// </summary>
    public float comboTime;
    private float _formComboBeforeThisTime;

    /// <summary>
    /// Array of skills (and basic attack) assosciated with this weapon
    /// Protocol: 
    /// [0 to comboLengthCapacity - 1] basic attack and the combo
    /// [comboLengthCapacity+] weapon skill
    /// </summary>
    public MeleeSkill[] skills;
    
    /// <summary>
    /// Array of skill IDs, used to auto initiate the weapon
    /// </summary>
    public int[] skillIds;

    // TODO: merge with skills
    public Skill[] abilities;
    public int[] abilityIds;

    

    // Use this for initialization
    public void Initialize(UnityEngine.UI.Text debugText = null)
    {
        _comboLength = 0;
        if (/*skills.Length == 0 && */skillIds.Length > 0)
        {
            // Not defined manually
            skills = new MeleeSkill[skillIds.Length];
            MeleeSkillLibrary meleeLibrary = GameObject.Find("Skill Library").GetComponent<MeleeSkillLibraryBehavior>().library;
            for (int i = 0; i < skills.Length; i++)
            {
                skills[i] = meleeLibrary.skills[skillIds[i]];
            }
        }

        if (abilities.Length == 0 && abilityIds.Length > 0)
        {
            #region OverrideForPlaytest
            if (abilityIds.Length == 1 && abilityIds[0] == 2016)
            {
                abilities = new Skill[1];
                SkillLibrary skillLibrary = GameObject.Find("Skill Library 2").GetComponent<SkillLibraryBehavior>().skillLibrary;
                abilities[0] = skillLibrary.blinkSkill;
            }
            #endregion
            else
            {
                abilities = new Skill[abilityIds.Length];
                SkillLibrary skillLibrary = GameObject.Find("Skill Library 2").GetComponent<SkillLibraryBehavior>().skillLibrary;
                if (skillLibrary.skills.Length >= abilityIds.Length)
                {
                    for (int i = 0; i < abilities.Length; i++)
                    {
                        abilities[i] = skillLibrary.skills[abilityIds[i]];
                    }
                }
            }
            
        }

        // Update basic attack icon
        string basicAttackButtonSpriteName = null;
        switch (type)
        {
            case WeaponType.AXE:
                basicAttackButtonSpriteName = "WeaponAttackAxe";
                break;
            case WeaponType.HAMMER:
                basicAttackButtonSpriteName = "WeaponAttackHammer";
                break;
            case WeaponType.SWORD:
                basicAttackButtonSpriteName = "WeaponAttackSword";
                break;
        }

        if (basicAttackButtonSpriteName != null)
        {
            GameObject basicAttackButton = GameObject.Find("Canvas/Basic Attack");
            var sprite = Resources.Load<Sprite>("Icons/" + basicAttackButtonSpriteName);
            if (sprite != null)
            {
                basicAttackButton.GetComponent<Image>().overrideSprite = sprite;
            }
        }

        // Update ability icons
        if (abilities.Length > 0)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject abilityButton = GameObject.Find("Canvas/Ability " + i);
                if (abilityButton != null)
                {
                    if (i < abilities.Length)
                    {
                        var sprite = Resources.Load<Sprite>("Icons/" + abilities[i].iconName);
                        abilityButton.GetComponent<Image>().overrideSprite = sprite;
                    }
                    else
                    {
                        abilityButton.GetComponent<Image>().overrideSprite = null;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void DebugLog()
    {
    }

    /// <summary>
    /// Use the weapon to do basic attack (or its combo)
    /// Return the self stagger time
    /// </summary>
    public virtual float WeaponAttack(Transform attackerTransform)
    {
        float staggerTime = skills[_comboLength++].CastSkill(attackerTransform);
        if (_comboLength >= comboLengthCapacity)
        {
            _comboLength = 0;
        }


        return 0;
    }

    /// <summary>
    /// Called when basic weapon attack is casted
    /// </summary>
    /// <returns>
    /// The MeleeSkill object represents the current basic weapon attack or combo
    /// </returns>
    public virtual MeleeSkill GetWeaponAttackSkill(out int weaponAttackIndex)
    {
        MeleeSkill skill = null;
        if (Time.time < _formComboBeforeThisTime)
        {
            weaponAttackIndex = _comboLength;
            skill = skills[_comboLength++];
            if (_comboLength >= comboLengthCapacity ||
                (comboLengthCapacity > skills.Length && _comboLength >= skills.Length))
            {
                _comboLength = 0;
            }
        }
        else
        {
            weaponAttackIndex = 0;
            skill = skills[0];
            _comboLength = 1;
        }
        _formComboBeforeThisTime = Time.time + comboTime;

        return skill;
    }

    /// <summary>
    /// Use the weapon to do basic attack (or its combo)
    /// Return the self stagger time
    /// </summary>
    public virtual float WeaponSkill()
    {
        float staggerTime = 0;
        return staggerTime;
    }

    public void UpdateAbilities(int ability0Index, int ability1Index)
    {
        abilityIds = new int[2];
        abilityIds[0] = ability0Index;
        abilityIds[1] = ability1Index;

        abilities = new Skill[abilityIds.Length];
        SkillLibrary skillLibrary = GameObject.Find("Skill Library 2").GetComponent<SkillLibraryBehavior>().skillLibrary;
        if (skillLibrary.skills.Length >= abilityIds.Length)
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                abilities[i] = skillLibrary.skills[abilityIds[i]];
            }
        }

        if (abilities.Length > 0)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject abilityButton = GameObject.Find("Canvas/Ability " + i);
                if (abilityButton != null)
                {
                    if (i < abilities.Length)
                    {
                        var sprite = Resources.Load<Sprite>("Icons/" + abilities[i].iconName);
                        abilityButton.GetComponent<Image>().overrideSprite = sprite;
                    }
                    else
                    {
                        abilityButton.GetComponent<Image>().overrideSprite = null;
                    }
                }
            }
        }
    }
}
