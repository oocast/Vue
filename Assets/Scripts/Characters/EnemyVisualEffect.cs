using UnityEngine;
using System.Collections;

public class EnemyVisualEffect : MonoBehaviour {
    public GameObject deathEffectPrefab;
	SoundSystem soundSystem;

	// Use this for initialization
	void Start () {
        EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.onDeath += InvokePlayDeathEffect;
        }
		var soundSystemObj = GameObject.Find("Sound System");
		if(soundSystemObj!=null)
		{
			soundSystem = soundSystemObj.GetComponent<SoundSystem>();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void PlayDeathEffect()
    {
		if(soundSystem!=null)
			soundSystem.PlaySound(transform.position,"Spider Death",0.05f);
        if (deathEffectPrefab != null)
        {
            GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
            Destroy(deathEffect, 2f);
        }
    }

    void InvokePlayDeathEffect()
    {
        Invoke("PlayDeathEffect", 0.4f);
    }
}
