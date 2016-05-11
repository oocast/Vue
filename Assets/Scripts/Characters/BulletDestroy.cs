using UnityEngine;
using System.Collections;

public class BulletDestroy : MonoBehaviour {

    int life = 2;
    GameObject player;
    PlayerHealth playerHealth;
    public int attackDamage;

    void Awake()
    {
        Destroy(gameObject, life);
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            playerHealth.TakeDamage(attackDamage);
        }

    }
}
