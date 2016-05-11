using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShockWaveBullet : MonoBehaviour {

    [HideInInspector]
    public int damage;

    [HideInInspector]
    public string targetTag;

    [HideInInspector]
    public float stunTime;
    public float moveDistance;


    HashSet<Collider> hittenTargets;


    // Use this for initialization
    void Start () {
        hittenTargets = new HashSet<Collider>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag && !other.isTrigger)
        {
            ICharacterHealth health = other.GetComponent<ICharacterHealth>();
            if (health != null && !hittenTargets.Contains(other))
            {
                health.TakeDamage(damage, stunTime, moveDistance, transform);
                hittenTargets.Add(other);
            }
            else
            {
                Debug.LogWarning("ShockWaveBullet: target has no health interface");
            }
        }
    }
}
