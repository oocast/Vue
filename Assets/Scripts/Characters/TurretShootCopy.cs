using UnityEngine;
using System.Collections;

public class TurretShootCopy : MonoBehaviour {
    public GameObject bullet;
    public GameObject barrelEnd;
    GameObject player;
    PlayerHealth playerHealth;
	SoundSystem soundSystem;
    private float InstantiationTimer = 2.0f;

    private bool hasCollide = false;
    private bool _didEnterHitArea = false;
    private int _bulletsShot = 0;
    private int seconds = 5;
    public int attackDamage = 5;
    public float inverseFireRate;
	float timer;
	public float firingRate;
	Collider[] playerCollider;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
		var soundSystemObj = GameObject.Find("Sound System");
		if(soundSystemObj!=null)
		{
			soundSystem = soundSystemObj.GetComponent<SoundSystem>();
		}
	}
    
	void Start()
	{
		timer = 0;
	}

	void Update()
	{
		playerCollider = Physics.OverlapSphere(gameObject.transform.position,GetComponent<SphereCollider>().radius * transform.lossyScale.x,2048);

		if(playerCollider.Length!=0)
		{	
			timer-=Time.deltaTime;
			_didEnterHitArea = true;
			if(timer<0.0005)
				StartCoroutine(CreatePrefab(inverseFireRate));
		}
		else
		{
			_didEnterHitArea = false;
		}
	}

    void resetCollider()
    {
        hasCollide = false;
    }

    IEnumerator CreatePrefab(float fireRate)
    {
        if (_didEnterHitArea)
        {
			timer = firingRate;
			GameObject instanceBullet;
			instanceBullet = Instantiate(bullet, barrelEnd.transform.position, barrelEnd.transform.rotation) as GameObject;

			#region Sound Effects
			if(soundSystem!=null)
			{
				if(gameObject.name.Contains("Turret"))
					soundSystem.PlaySound(transform.position,"Turret Fire");
				if(gameObject.name.Contains("Tank"))
					soundSystem.PlaySound(transform.position,"Tank Fire");
			}
			#endregion
			if(instanceBullet.GetComponent<Rigidbody>()!=null)
				instanceBullet.GetComponent<Rigidbody>().AddForce(barrelEnd.transform.forward * 700);
        }
        yield return null;
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
