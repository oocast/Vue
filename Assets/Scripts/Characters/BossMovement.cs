using UnityEngine;
using System.Collections;

public class BossMovement : MonoBehaviour {
    public event System.Action<float> onMoveAtSpeed;

    Transform player;
    PlayerHealth playerHealth;
   // EnemyHealth enemyHealth;
    NavMeshAgent navAgent;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        //enemyHealth = GetComponent<EnemyHealth>();
        navAgent = GetComponent<NavMeshAgent>();

    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ( playerHealth.currentHealth > 0)
        {
            //nav mesh agent to set to new position.. yet to fdigure out
            navAgent.destination = player.position;
            if (navAgent.remainingDistance < navAgent.stoppingDistance)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position, transform.up), Time.deltaTime * navAgent.angularSpeed / 360f);
            }
        }
        else
        {
            //nav.enabled=false;
            navAgent.Stop();
        }

        if (onMoveAtSpeed != null)
        {
            onMoveAtSpeed(navAgent.velocity.magnitude);
        }

    }
}
