using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EnemyHealth : MonoBehaviour, ICharacterHealth {
    public event System.Action onDeath;

	public int startingHealth=100;
	public int currentHealth;
	public int scoreValue=10;//amount added to player's score whehn enemy dies
	bool isDead = false;
	SoundSystem soundSystem;

	void Awake()
	{
		currentHealth = startingHealth;
		var soundSystemObj = GameObject.Find("Sound System");
		if(soundSystemObj!=null)
		{
			soundSystem = soundSystemObj.GetComponent<SoundSystem>();
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TakeDamage(int amount)
    {
        if (isDead)
        {
            return;
        }
        //play enemy hurt sound
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Death();
        }
    }


    public void TakeDamage(int amount, float stunTime, float moveDistance, Transform source)
	{
		if (isDead) {
			return;
		}
		//play enemy hurt sound
		currentHealth -= amount;

		if(currentHealth <=0)
		{
			Death();
		}

        EnemyAttack enemyAttack = GetComponent<EnemyAttack>();
        if (enemyAttack != null && stunTime > 0f)
        {
            enemyAttack.SetStun(stunTime);
        }
        EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.SetStunAndKnockBack(stunTime, moveDistance, source);
        }

        // Blink object
        Renderer[] meshRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer meshRenderer in meshRenderers)
        {
            foreach (Material material in meshRenderer.materials)
            {
                Color color = material.color;
                material.DOColor(Color.white, 0.08f);
                material.DOColor(color, 0.08f).SetDelay(0.08f);
            }
        }
	}

	void Death()
	{
		isDead = true;
		if(soundSystem!=null)
		{
			if(gameObject.name.Contains("Turret"))
				soundSystem.PlaySound(transform.position,"Turret Destroy");
			if(gameObject.name.Contains("Tank"))
				soundSystem.PlaySound(transform.position,"Tank Destroy");
	    }
		//turn collider of the enemy into a trigger so that shots can pass through it now
		//play dead sound
		Destroy (gameObject, 0.5f);
        if (onDeath != null)
        {
            onDeath();
        }
    }

    public void TakeHeal(int amount)
    {
        if (isDead)
        {
            return;
        }

    }

    void OnDestroy()
    {
        
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
