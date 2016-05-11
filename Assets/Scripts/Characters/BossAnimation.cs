using UnityEngine;
using System.Collections;

public class BossAnimation : MonoBehaviour, ICharacterHealth {
    public event System.Action onDeath;

    public int startingHealth = 100;
    public int currentHealth;
    public int scoreValue = 10;//amount added to player's score whehn enemy dies
    bool isDead = false;
    bool flag = false;
    Animator animator;
    public int StartBeserkFirst;
    public int StartBeserkSecond;
    public int BeserkFirstThreshold;
    public int BeserkSecondThreshold;

    void Awake()
    {
        currentHealth = startingHealth;
    }
    // Use this for initialization
    void Start()
    {
       // animator = GetComponent<Animator>();
        animator = GetComponentInChildren<Animator>();
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
        {
            return;
        }
        //play enemy hurt sound
        currentHealth -= amount;
        print("currentHealth: "+currentHealth);
        if ((currentHealth >= StartBeserkFirst) && (currentHealth <= BeserkFirstThreshold))
        {
            print("asd");
            flag = true;
            animator.SetTrigger("Beserk");
            flag = false;
        }
        else if ((currentHealth >= StartBeserkSecond) && (currentHealth <= BeserkSecondThreshold)) 
        {
            flag = true;
            animator.SetTrigger("Beserk");
            flag = false;
        }
        
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void TakeDamage(int amount, float stunTime, float moveDistance, Transform source)
    {
        if (isDead)
        {
            return;
        }
        //play enemy hurt sound
        currentHealth -= amount;
        print("currentHealth: " + currentHealth);
        if ((currentHealth >= StartBeserkFirst) && (currentHealth <= BeserkFirstThreshold))
        {
            print("asd");
            flag = true;
            animator.SetTrigger("Beserk");
            flag = false;
        }
        else if ((currentHealth >= StartBeserkSecond) && (currentHealth <= BeserkSecondThreshold))
        {
            flag = true;
            animator.SetTrigger("Beserk");
            flag = false;
        }

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;

        //turn collider of the enemy into a trigger so that shots can pass through it now
        //play dead sound
        Destroy(gameObject, 0.5f);
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

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
