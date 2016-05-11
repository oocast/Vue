using UnityEngine;
using System.Collections.Generic;

public class Charge : MonoBehaviour {
    GameObject chargeTriggerObject = null;
    SphereCollider chargeTrigger = null;
    float _chargeSpeed = 0f;
    float _chargeDamageRadius;
    int _chargeDamage;
    float _stunTime;
    int oldLayer;

    HashSet<Collider> hittenTargets;

    public void Initialize(float chargeSpeed, int[] damageList, float stunTime)
    {
        _chargeSpeed = chargeSpeed;
        oldLayer = gameObject.layer;
        _chargeDamage = damageList[0];
        _stunTime = stunTime;
        gameObject.layer = LayerMask.NameToLayer("Charging");
    }

	// Use this for initialization
	void Start () {
        _chargeDamageRadius = 1.5f;
        chargeTriggerObject = new GameObject();
        chargeTriggerObject.transform.SetParent(transform);
        chargeTriggerObject.layer = LayerMask.NameToLayer("Player");
        chargeTriggerObject.name = "Charge Trigger Object";
        chargeTriggerObject.transform.position = transform.position;
        chargeTrigger = chargeTriggerObject.AddComponent<SphereCollider>();
        chargeTrigger.isTrigger = true;
        chargeTrigger.radius = _chargeDamageRadius;
        hittenTargets = new HashSet<Collider>();
    }
	
	// Update is called once per frame
	void Update () {
        CharacterController controller = GetComponent<CharacterController>();
	    if (controller != null)
        {
            controller.SimpleMove(transform.forward * _chargeSpeed);
        }
	}

    void OnDestroy()
    {
        gameObject.layer = oldLayer;
        if (chargeTriggerObject != null)
        {
            Destroy(chargeTriggerObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && !other.isTrigger)
        {
            ICharacterHealth health = other.GetComponent<ICharacterHealth>();
            if (health != null && !hittenTargets.Contains(other))
            {
                health.TakeDamage(_chargeDamage, _stunTime, 2f, transform);
                hittenTargets.Add(other);
            }
        }
    }
}
