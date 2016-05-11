using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
    private float _runningFactor = 4f;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        //GameObject.Find("Character").GetComponent<CharacterMovement>().onCharacterMoveSpeed += UpdateRunningSpeed;
    }

    // TODO: use blend tree
    public void UpdateRunningSpeed(float dragVectorMagnitude)
    {
        // animator.speed = dragVectorMagnitude * _runningFactor;
        animator.SetFloat("Speed", dragVectorMagnitude);
    }

    public void SetWeaponAttack(int index)
    {
        if (index > 1 || index < 0)
        {
            return;
        }
        animator.SetTrigger("Weapon Attack " + (index + 1));
    }

    public void SetWeaponTrail(bool active)
    {
        TrailRenderer trailRenderer = GetComponentInChildren<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.enabled = active;
        }
    }

    public void DisableWeaponTrail()
    {
        TrailRenderer trailRenderer = GetComponentInChildren<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
    }

    public void EnableWeaponTrail()
    {
        TrailRenderer trailRenderer = GetComponentInChildren<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
        }
    }
}
