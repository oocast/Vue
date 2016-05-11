using UnityEngine;
using System.Collections;

public class TriggerBoxOtherEnemy : MonoBehaviour {

    //public GameObject prefab;
    public GameObject[] enemies;
    public int number_of_enemies;
    Vector3 spawn_point;
    public float time_interval;
    private float time_temp;
    public GameObject dome;
    Transform dome_child;
    //int num = enemies.Length;
    // Use this for initialization
    void Start()
    {
        dome_child = transform.Find("group4_pSphere3");
        if (dome_child == null)
        {
            dome_child = transform.GetChild(0).Find("group4_pSphere3");
        }
        spawn_point = dome_child.position;
        time_temp = time_interval;
    }

    // Update is called once per frame
    void Update()
    {
        enemy_time_spawner();
    }

    void enemy_time_spawner()
    {
        time_interval -= Time.deltaTime;
        if (time_interval <= 0)
        {
            for (int i = 0; i < number_of_enemies; i++)
            {
                for (int j = 0; j < enemies.Length; j++)
                {
                    Instantiate(enemies[j], spawn_point, Quaternion.identity);
                }
            }
            time_interval = time_temp;
        }
    }
}
