using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
/// <summary>
/// Support only circular (sector) shape for this version
/// </summary>
public class AOESkill : Skill {
    /// <summary>
    /// Multiple radii match elements in damageList
    /// From inside to outside
    /// </summary>
    public float[] radii;

    /// <summary>
    /// The offset between referenceTransform and the AOE center
    /// </summary>
    public Vector3 centerOffset;

    /// <summary>
    /// The sector use forward as reference or backward as reference
    /// </summary>
    public bool direction;

    /// <summary>
    /// The angle between the reference direction vector and referenceTransform-enemyTransform vector
    /// -angle to angle
    /// </summary>
    public float angle;

    public AOESkill()
    {
        radii = new float[1];
    }

    public override void UpdateEffect(Transform referenceTransform = null, GameObject skillCaster = null)
    {
        if (referenceTransform == null)
        {
            Debug.LogWarning("AOESkill:UpdateEffect referenceTransform is null");
            return;
        }
        if (radii.Length != damageList.Length && damageList.Length < 1)
        {
            Debug.LogWarning("AOESkill:UpdateEffect damgeList and radii mismatch");
            return;
        }

    }

    public List<GameObject>[] GetTargetLists(Transform referenceTransform = null)
    {

        List<GameObject>[] result = new List<GameObject>[radii.Length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = new List<GameObject>();
        }

        // TODO: Get with physics overlap
        GameObject[] targetsByTag = GameObject.FindGameObjectsWithTag(targetTag);

        if (referenceTransform != null)
        {
            foreach (GameObject target in targetsByTag)
            {
                Collider[] targetColliders = target.GetComponents<Collider>();
                Collider targetCollider = null;
                foreach (Collider collider in targetColliders)
                {
                    if (!collider.isTrigger)
                    {
                        targetCollider = collider;
                        break;
                    }
                }
                if (targetCollider == null)
                {
                    continue;
                }
                Vector3 closestPosition = targetCollider.bounds.ClosestPoint(referenceTransform.position);

                // check angle
                if (Vector3.Angle(referenceTransform.position.Direction2D(closestPosition), referenceTransform.forward) < angle)
                {
                    for (int i = 0; i < radii.Length; i++)
                    {
                        float innerRadius = i == 0 ? 0 : radii[i - 1];
                        if (referenceTransform.position.SquareDistance2D(closestPosition) < radii[i] * radii[i]
                            && referenceTransform.position.SquareDistance2D(closestPosition) > innerRadius * innerRadius)
                        {
                            result[i].Add(target);
                            break;
                        }
                    }
                }
            }
        }
        return result;
    }

    public void CauseDamage(ICollection<GameObject>[] targetLists, Transform source)
    {
        if (targetLists.Length > damageList.Length)
        {
            return;
        }

        for (int i = 0; i < targetLists.Length; i++)
        {
            foreach (GameObject target in targetLists[i])
            {
                ICharacterHealth health = target.GetComponent<ICharacterHealth>();
                if (health != null)
                {
                    health.TakeDamage(damageList[i], stunTime, moveDistance, source);
                }
            }
        }
    }
}
