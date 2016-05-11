using UnityEngine;
using System.Collections;

public class TriggerBox : MonoBehaviour {
    //public GameObject prefab;
    public GameObject[] enemies;
    public int number_of_enemies;
    public Transform spawn_point;
    public EnemySummon enemySummon;
    bool flag = false;
    //int num = enemies.Length;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider triggerBox)
    {
        // Debug.Log("trigger!!!! detected");
        if (triggerBox.gameObject.tag == "Player")
        {
            Debug.Log("trigger detected");
            while(flag==false)
            {

                flag = true;
                for (int i = 0; i < number_of_enemies; i++)
                {
                    for (int j = 0; j < enemies.Length; j++)
                    {
                        Instantiate(enemies[j], spawn_point.position, Quaternion.identity);
                    }
                }
                
				//Deprecated
				/*
                if (enemySummon != null)
                {
                    enemySummon.SummonEnemy(spawn_point);
                    enemySummon = null;
                }*/
            }
            
        }
    }
}
