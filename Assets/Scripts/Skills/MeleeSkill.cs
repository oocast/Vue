using UnityEngine;
using System.Collections;

[System.Serializable]
public class MeleeSkill
{
    #region identity
    public string skillName;
    public int skillID;
    #endregion

    #region skillRange
    /// <summary>
    /// The effective hit sector tangential is (-halfBrandishAngle, halfBrandishAngle)
    /// </summary>
    public float halfBrandishAngle;

    /// <summary>
    /// The effective hit sector region normal range is (brandishDistanceMin, brandishDistanceMax)
    /// </summary>
    public float brandishDistanceMin;
    public float brandishDistanceMax;

    // TODO: define brandish rotational direction
    #endregion

    #region skillTime
    /// <summary>
    /// The time between skill cast and hit
    /// </summary>
    public float hitTime;

    /// <summary>
    /// The pre-casting time of the skill
    /// </summary>
    public float castingTime;

    /// <summary>
    /// The interval between consecutive skill casts
    /// </summary>
    public float coolDownTime;

    /// <summary>
    /// The stagger time caused on characters casting the skill
    /// Disable all skills (weapon or personal) for selfStaggerTime 
    /// </summary>
    public float selfStaggerTime;

    /// <summary>
    /// The stagger time caused on targets being hit
    /// </summary>
    public float targetStaggerTime;

    // TODO: cannotMoveTime: just cannot move, but can cast different skill
    #endregion

    /// <summary>
    /// Damage caused by this skill
    /// </summary>
    public int damage;

    /// <summary>
    /// Whether the weapon holder can use this melee skill to attack while moving
    /// </summary>
    public bool moveAttack;

    public float moveDistance;

    public float CastSkill(Transform casterTransform)
    {
        return selfStaggerTime;
    }
}
