using UnityEngine;
using System.Collections;

public class DetectPlayer : MonoBehaviour {
	public event System.Action<float> onMoveAtSpeed;

    GameObject player;
    public float rangeToAvoidPlayer;
    private float dist;
	NavMeshAgent navAgent;
	NavMeshPath lastAgentPath;
	float lastAgentVelocity;

    // Use this for initialization
    void Start () {

        lastAgentPath = new NavMeshPath();
        player = GameObject.FindGameObjectWithTag("Player");
        navAgent = GetComponent<NavMeshAgent>();
        dist = Vector3.Distance(this.transform.position, player.transform.position);
    }
	
	// Update is called once per frame
	void Update () {
        dist = Vector3.Distance(this.transform.position, player.transform.position);
        if (dist<=rangeToAvoidPlayer)
        {
           // gameObject.GetComponent<EnemyMovement>().enabled = false;
			navAgent.Stop ();
        }
        if (dist > rangeToAvoidPlayer)
        {
           // gameObject.GetComponent<EnemyMovement>().enabled = true;
			navAgent.Resume ();
        }

    }

	/*void pause()
	{
		lastAgentVelocity = navAgent.velocity;
		lastAgentPath = navAgent.path;
		navAgent.velocity = 0;
		navAgent.ResetPath ();
	}

	void Resume()
	{
		navAgent.velocity = lastAgentVelocity;
		navAgent.SetPath = lastAgentPath;
	}*/
}
