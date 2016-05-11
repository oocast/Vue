using UnityEngine;
using System.Collections;

public class EnemyAnimationToPoint : MonoBehaviour {
    Animator animator;

    void Awake()
    {
        GetComponentInParent<EnemyMovementToGoal>().onMoveAtSpeed += SetMoveSpeed;
        GetComponentInParent<EnemyHealth>().onDeath += SetDeath;
        GetComponentInParent<EnemyAttack>().onAttack += SetAttack;

    }

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not attached!");
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetMoveSpeed(float speed)
    {
        animator.SetFloat("MoveSpeed", speed);
    }

    void SetAttack()
    {
        animator.SetTrigger("Attack");
    }

    void SetDeath()
    {
        animator.SetTrigger("Death");
    }

    void SetStun(bool stun)
    {
        animator.SetBool("Stun", stun);
    }
}
