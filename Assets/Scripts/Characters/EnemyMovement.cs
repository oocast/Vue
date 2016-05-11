using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EnemyMovement : MonoBehaviour {
    public event System.Action<float> onMoveAtSpeed;
    public event System.Action<bool> onStun;

    Transform player;
	PlayerHealth playerHealth;
	EnemyHealth enemyHealth;
    NavMeshAgent navAgent;

    float stunEndTime = 0f;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		playerHealth = player.GetComponent <PlayerHealth > ();
		enemyHealth = GetComponent <EnemyHealth> ();
        navAgent = GetComponent<NavMeshAgent>();

    }
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0
            && navAgent != null && player != null) {
            //nav mesh agent to set to new position.. yet to fdigure out
            navAgent.destination = player.position;
            if (navAgent.remainingDistance < navAgent.stoppingDistance)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position, transform.up), Time.deltaTime * navAgent.angularSpeed / 360f);
            }
        } 
		else {
            //nav.enabled=false;
            navAgent.Stop();

        }

        if (onMoveAtSpeed != null)
        {
            onMoveAtSpeed(navAgent.velocity.magnitude);
        }

	}

    public void SetStunAndKnockBack(float stunTime, float moveDistance, Transform source)
    {
        float restartTime = stunTime > 0.5f ? stunTime : 0.5f;
        if (navAgent != null)
        {
            navAgent.Stop();
        }
        Vector3 direction = (transform.position - source.position);
        direction.Normalize();
        GetComponent<Rigidbody>().DOMove(transform.position + direction * moveDistance, 0.5f);

        if (Time.time + restartTime > stunEndTime)
        {
            StopCoroutine("RestartNavAgent");
            StartCoroutine("RestartNavAgent", restartTime);
            if (onStun != null)
            {
                onStun(true);
            }
            stunEndTime = Time.time + restartTime;
        }
    }

    IEnumerator RestartNavAgent(float restartTime)
    {
        yield return new WaitForSeconds(restartTime);
        if (navAgent != null)
        {
            navAgent.Resume();
        }
        if (onStun != null)
        {
            onStun(false);
        }
    }

}
