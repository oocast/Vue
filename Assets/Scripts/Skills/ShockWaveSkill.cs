using UnityEngine;
using System.Collections;
using DG.Tweening;

[System.Serializable]
public class ShockWaveSkill : Skill {
    public string bulletPrefabName;

    public float distance;

    public float bulletMovingTime;

    public ShockWaveSkill()
    {
        bulletPrefabName = "Shock Wave Bullet";
    }

    public override void UpdateEffect(Transform referenceTransform = null, GameObject skillCaster = null)
    {
        if (referenceTransform == null)
        {
            Debug.LogWarning("ShockWaveSkill:UpdateEffect referenceTransform is null");
            return;
        }
        if (damageList.Length < 1)
        {
            Debug.LogWarning("ShockWaveSkill:UpdateEffect damageList too short");
            return;
        }

        // create shock wave object
        GameObject bulletPrefab = Resources.Load("Prefabs/" + bulletPrefabName) as GameObject;
        Vector3 position = referenceTransform.position;
        GameObject bullet = Object.Instantiate(bulletPrefab, position, Quaternion.LookRotation(referenceTransform.forward)) as GameObject;
        var bulletScript = bullet.GetComponent<ShockWaveBullet>();
        bulletScript.damage = damageList[0];
        bulletScript.targetTag = targetTag;
        Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.DOMove(bullet.transform.position + referenceTransform.forward * distance, bulletMovingTime);
        }
        else
        {
            bullet.transform.DOMove(bullet.transform.position + referenceTransform.forward * distance, bulletMovingTime);
        }
        
        bullet.GetComponentInChildren<ParticleSystem>();
        GameObject.Destroy(bullet.GetComponent<Collider>(), bulletMovingTime);
        GameObject.Destroy(bullet, bulletMovingTime * 2f);
    }

    public override void OnActivate(GameObject skillCaster)
    {
        base.OnActivate(skillCaster);
        if (targetTag.Equals("Enemy"))
        {
            Animator animator = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.SetTrigger(skillName);
            }
        }
    }
}
