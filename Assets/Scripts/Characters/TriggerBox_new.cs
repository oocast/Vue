using UnityEngine;
using System.Collections;

public class TriggerBox_new : MonoBehaviour {
	
	public GameObject[] enemies;
	public int number_of_enemies;
	public Transform spawn_point;
	public EnemySummon enemySummon;
	bool flag = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider triggerBox)
	{
		if (triggerBox.gameObject.tag == "Player")
		{
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
				

                if (enemySummon != null)
                {
                    enemySummon.SummonEnemy(spawn_point.position);
                    enemySummon = null;
                }
			}
			
		}
	}
}
