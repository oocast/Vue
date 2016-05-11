using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EnemyBombBehavior : MonoBehaviour {
	public float fuseTime;
	public float explosionRadius;
	public int damage;
	public GameObject explosionEffect;
	private GameObject target;
	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player");
		SetFuse (fuseTime);
        SetPath();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetPath()
    {
        Vector3[] positions = new Vector3[3];
        positions[0] = transform.position;
        positions[2] = target.transform.position - transform.forward * 2f;
        positions[1] = (positions[0] + positions[2]) / 2f;
        positions[1].y += 5f;
        transform.DOPath(positions, 0.6f, PathType.CatmullRom);
    }

	void Explode() {
		if(Vector3.Distance(transform.position,target.transform.position) <= explosionRadius){
			target.GetComponent<PlayerHealth> ().TakeDamage (damage);
		}
	}
	public void SetFuse(float time){
		Invoke ("Explode", time);
		Destroy (gameObject, 2f);
	}

	public void OnDestroy() {
		GameObject effect = Instantiate(explosionEffect, transform.position + new Vector3(0, 2, 0), Quaternion.identity) as GameObject;
		Destroy (effect, 2f);
	}
}
