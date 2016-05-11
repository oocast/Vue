using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHealth : MonoBehaviour {

	public int startingHealth=100;
	public int currentHealth;
	public Slider healthSlider;
    public int threshold;
    public Image BloodOverlay;
    /// <summary>
    /// Recover amount of HP each second
    /// </summary>
    public int healRate;

	CharacterMovement playerMovement;
	CharacterAttack playerAttack;
	bool isDead;
	bool damaged;


	void Awake()
	{
		playerMovement = GetComponent <CharacterMovement > ();
		playerAttack = GetComponent <CharacterAttack> ();
        //BloodOverlay = GetComponent<Image>();
        //currentHealth = startingHealth;
        if (BloodOverlay != null)
        {
            BloodOverlay.enabled = false;
        }
    }
    // Use this for initialization
    void Start () {
        currentHealth = startingHealth;
        StartCoroutine("ContinuousHeal");
    }
	
	// Update is called once per frame
	void Update () {
		if (damaged) {
			//maybe change the animation to show damaged
		}
		damaged = false;

        if(currentHealth<=threshold && BloodOverlay != null)
        {
            BloodOverlay.enabled = true;
            float alpha = (float)(threshold - currentHealth) / threshold;
            Color color = BloodOverlay.color;
            color.a = alpha;
            BloodOverlay.color = color;
            // BloodOverlay.DOFade(alpha, 0.05f);

            GameObject.Find("Sound System").GetComponent<CharacterHeartBeat>().UpdateFrequency(alpha);
        }
        else if (currentHealth > threshold && BloodOverlay != null)
        {
            BloodOverlay.enabled = false;
            GameObject.Find("Sound System").GetComponent<CharacterHeartBeat>().UpdateFrequency(0f);
        }
	}

	public void TakeDamage(int amount)
	{
        //Debug.Log("in player health");
        //Debug.Log("damage:"+amount);
        damaged = true;
		currentHealth = currentHealth- amount;
        //Debug.Log(currentHealth);
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
		//play hurt sound

		if (currentHealth <= 0 && ! isDead) {
			Death ();
		}
	}

    IEnumerator ContinuousHeal()
    {
        while (true && healRate > 0)
        {
            if (currentHealth < startingHealth)
            {
                currentHealth++;
            }
            yield return new WaitForSeconds(1f / (float)healRate);
        }
    }

	void Death()
	{
		isDead = true;
		//turn off all shooting in CharacterAttack file
		//animation trigger set to "die"
		playerMovement.enabled = false;
		playerAttack.enabled = false;
        GameObject.Find("Canvas/Basic Attack").SetActive(false);
        GameObject.Find("Canvas/Ability 0").SetActive(false);
        GameObject.Find("Canvas/Ability 1").SetActive(false);
        GameObject.Find("Canvas/Thumbstick Background").SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
        Invoke("Reload", 4);
	}

    void Reload()
    {
        Application.LoadLevel(Application.loadedLevel);
        
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

}
