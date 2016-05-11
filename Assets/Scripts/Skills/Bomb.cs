using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
    public GameObject explosionEffect;
    BombSkill bombSkill;

    public void Initialize(BombSkill bombSkill)
    {
        this.bombSkill = bombSkill;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void Explode()
    {
        var targetLists = bombSkill.GetTargetLists(transform);
        bombSkill.CauseDamage(targetLists, transform);
    }

    public void SetFuse(float time)
    {
        Invoke("Explode", time);
        Destroy(gameObject, time);
    }

    public void OnDestroy()
    {
        GameObject effect = Instantiate(explosionEffect, transform.position + new Vector3(0, 2, 0), Quaternion.identity) as GameObject;
        Destroy(effect, 2f);
    }
}
