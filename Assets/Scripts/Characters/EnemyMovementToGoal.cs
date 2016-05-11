using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EnemyMovementToGoal : MonoBehaviour {
    public event System.Action<float> onMoveAtSpeed;
    public event System.Action<bool> onStun;
	Transform target;

    Transform player;
	PlayerHealth playerHealth;
	EnemyHealth enemyHealth;
    NavMeshAgent navAgent;

    float stunEndTime = 0f;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		target = GameObject.FindGameObjectWithTag ("SpiderTarget").transform;
		playerHealth = player.GetComponent <PlayerHealth > ();
		enemyHealth = GetComponent <EnemyHealth> ();
        navAgent = GetComponent<NavMeshAgent>();
		navAgent.destination = target.position;

    }
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
		float distance = Vector3.Distance (transform.position, target.position);
		if(distance < 25.0f) {
			Destroy (gameObject);
		}

	}

    public void SetStunAndKnockBack(float stunTime, float moveDistance, Transform source)
    {
        float restartTime = stunTime > 0.5f ? stunTime : 0.5f;
        navAgent.Stop();
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
        navAgent.Resume(); 
        if (onStun != null)
        {
            onStun(false);
        }
    }

}
