using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossAttack : MonoBehaviour
{
    public event System.Action onAttack;

    public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
    public int attackDamage = 10;               // The amount of health taken away per attack.

    GameObject player;                          // Reference to the player GameObject.
    PlayerHealth playerHealth;                  // Reference to the player's health.
    // EnemyHealth enemyHealth;                    // Reference to this enemy's health.
    ICharacterHealth enemyHealth;               // Reference to this enemy's health.
    bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
    float timer;                                // Timer for counting up to the next attack.
    bool attackLock;                            // During lock is true, no attack can be done.

    float stunEndTime = 0f;                     // time when the current stun end

    void Awake()
    {
        // Setting up the references.
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<ICharacterHealth>();
    }


    void OnTriggerEnter(Collider other)
    {
        // If the entering collider is the player...
        if (other.gameObject == player)
        {
            // ... the player is in range.
            playerInRange = true;
            Attack();
        }
    }


    void OnTriggerExit(Collider other)
    {
        // If the exiting collider is the player...
        if (other.gameObject == player)
        {
            // ... the player is no longer in range.
            playerInRange = false;
        }
    }


    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.GetCurrentHealth() > 0)
        {
            // ... attack.
            // Attack();
        }
    }


    void Attack()
    {
        // Reset the timer.
        timer = 0f;

        // If the player has health to lose...
        if (playerHealth.currentHealth > 0)
        {
            // ... damage the player.
            playerHealth.TakeDamage(attackDamage);

            if (onAttack != null)
            {
                onAttack();
            }
        }
    }

    public void SetStun(float stunTime)
    {
        if (Time.time + stunTime > stunEndTime)
        {
            StopCoroutine("Stun");
            StartCoroutine("Stun", stunTime);
            stunEndTime = Time.time + stunTime;
        }
    }

    IEnumerator Stun(float stunTime)
    {
        attackLock = true;
        yield return new WaitForSeconds(stunTime);
        attackLock = false;
    }

    /*	public float timeBetweenAttacks=0.5f;
        //public int attackDamage=10;
       // public Text timerLabel;

        GameObject player;
        PlayerHealth playerHealth;
        EnemyHealth enemyHealth;
        bool playerInRange;//to check if player is in enemy's proximity to attack
        //public float timer;//timer to count to next attack
        NormalSword sampleSword;
        // private MeleeSkill _basicAttackSkill = null;
        public int damage;

        void Awake()
        {
            player = GameObject .FindGameObjectWithTag ("Player");
            playerHealth = player.GetComponent <PlayerHealth > ();
            enemyHealth = GetComponent <EnemyHealth> ();
        }
        // Use this for initialization
        void Start () {

        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == player) {//if entering collider is of player, player is coming close

                playerInRange =true;
                Attack();
                playerInRange = false;
            }
        }

        void OntriggerExit(Collider other)
        {
            if (other.gameObject == player)//if exiting collider is of player, player is going away
                playerInRange = false;
        }


        /*
        // Update is called once per frame
        void Update () {


            if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0) {
                Attack();
            }
            if (playerHealth .currentHealth <= 0) {
                //set animation to dead by setting trigger
            }
        }

        void Attack()
        {
            //timer = 0f;
            if (playerHealth .currentHealth > 0) {
                Debug.Log(" player health >0 ");
                if (damage != null)
                {//if player can lose health
                    Debug.Log("calling player health");
                    playerHealth.TakeDamage(damage);//damage the player
                }
            }
        }
    */



}
