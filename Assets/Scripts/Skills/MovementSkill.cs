using UnityEngine;
using System.Collections;
using DG.Tweening;

[System.Serializable]
public class MovementSkill : Skill {
    public bool isBlink;
    public string effectName;
    float step;
    bool colorChanging = false;

    public MovementSkill()
    {
        disableSkill = true;
        disableMove = true;
        step = 0f;
    }

    override public void UpdateEffect(Transform referenceTransform = null, GameObject skillCaster = null)
    {
        if (skillCaster == null)
        {
            Debug.LogWarning("MovementSkill:UpdateEffect skillCaster is null");
            return;
        }
        if (period > 0.00001f)
        {
            return;
        }

        // Vector3 landingPosition = referenceTransform.position + moveVector;
        if (isBlink)
        {
            // change position one time
            if (step < 0.0001f)
            {
                var characterController = skillCaster.GetComponent<CharacterController>();
                var navMeshAgent = skillCaster.GetComponent<NavMeshAgent>();
                if (characterController != null)
                {
                    step = characterController.radius * 2f;
                }
                else if (navMeshAgent != null)
                {
                    step = navMeshAgent.radius * 2f;
                }
                else
                {
                    return;
                }
            }

            bool landingPositionConfirmed = false;
            float blinkDistance = moveDistance;

            Vector3 landingPosition = skillCaster.transform.position;
            while (!landingPositionConfirmed)
            {
                Vector3 moveVector = skillCaster.transform.forward * blinkDistance;
                Vector3 raySrartPoint = skillCaster.transform.position + moveVector;
                raySrartPoint.y = 100f;
                RaycastHit hit;

                if (Physics.Raycast(raySrartPoint, Vector3.down, out hit, 200f/*, LayerMask.GetMask("Ground", "Enemy", "Player")*/))
                {
                    int layerMask = LayerMask.GetMask("Ground", "Enemy", "Player");
                    string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                    int colliderLayerMask = LayerMask.GetMask(layerName);
                    if ((colliderLayerMask & layerMask) != 0)
                    {
                        landingPositionConfirmed = true;
                        landingPosition = hit.point;
                        landingPosition.y += 0.5f;
                    }
                }
                blinkDistance -= step;
                if (blinkDistance < 1f)
                {
                    landingPositionConfirmed = true;
                }
            }

            SetBlinkVisualEffect(skillCaster.transform.position, landingPosition, skillCaster);
            skillCaster.transform.position = landingPosition;
            // disapear()
            // appear()
        }
        else
        {
            // change position multiple times
            float moveSpeed = moveDistance / duration;
            Charge charge = skillCaster.AddComponent<Charge>();
            charge.Initialize(moveSpeed, damageList, stunTime);
            GameObject.Destroy(charge, duration);
        }
        // TODO: fix the position to
    }

    void SetBlinkVisualEffect(Vector3 startPosition, Vector3 endPosition, GameObject skillCaster)
    {
        GameObject blinkEffect = Resources.Load<GameObject>("Prefabs/" + effectName);
        if (blinkEffect != null)
        {
            // Set particle effect
            GameObject startEffect = Object.Instantiate(blinkEffect, startPosition, Quaternion.identity) as GameObject;
            GameObject endEffect = Object.Instantiate(blinkEffect, endPosition, Quaternion.identity) as GameObject;
            Object.Destroy(startEffect, 2f);
            Object.Destroy(endEffect, 2f);

            // Change character color
            if (!colorChanging)
            {
                colorChanging = true;
                Color blinkColor = blinkEffect.GetComponent<ParticleSystem>().startColor;
                Renderer[] renderers = skillCaster.transform.GetChild(0).GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    foreach (Material material in renderer.materials)
                    {
                        Color oldColor = material.color;
                        if (oldColor != null)
                        {
                            material.DOColor(blinkColor, 0.05f);
                            material.DOColor(oldColor, 0.05f).SetDelay(0.2f);
                        }
                    }
                }
                colorChanging = false;
            }
        }
    }
}
