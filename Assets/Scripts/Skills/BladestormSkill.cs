using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BladestormSkill : AOESkill {
    SoundSystem _sound;
    GameObject _soundEffectObject;

    public override void UpdateEffect(Transform referenceTransform = null, GameObject skillCaster = null)
    {
        ICollection<GameObject>[] targetList = GetTargetLists(referenceTransform);
        CauseDamage(targetList, referenceTransform);
    }

    public override void OnActivate(GameObject skillCaster)
    {
        if (targetTag.Equals("Enemy"))
        {
            Animator animator = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.SetBool(skillName, true);
                SoundSystem sound = GameObject.Find("Sound System").GetComponent<SoundSystem>();
                if (sound != null)
                {
                    _soundEffectObject = sound.PlayLoopingSound(Camera.main.transform.position, iconName);
                }
            }
        }

        if (skillCaster == GameObject.FindGameObjectWithTag("Player"))
        {
            if (disableSkill == true)
            {
                skillCaster.GetComponent<CharacterAttack>().LockAttack();
            }
        }
    }

    public override void OnDeactivate(GameObject skillCaster)
    {
        if (targetTag.Equals("Enemy"))
        {
            Animator animator = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.SetBool(skillName, false);
                if (_soundEffectObject != null)
                {
                    GameObject.Destroy(_soundEffectObject);
                }
            }
        }

        if (skillCaster == GameObject.FindGameObjectWithTag("Player"))
        {
            if (disableSkill == true)
            {
                skillCaster.GetComponent<CharacterAttack>().UnlockAttack();
            }
        }
    }
}
