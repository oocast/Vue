using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public GameObject player;
    PlayerHealth ph;
    Image progressBar;
    float percentage;
    float switchTime;

    // Use this for initialization
    void Start()
    {
        percentage = 0f;
        progressBar = GetComponent<Image>();
        progressBar.fillAmount = 1f;
        ph = player.GetComponent<PlayerHealth>();
        percentage = ph.currentHealth;
        Debug.Log("starting health"+ percentage);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFilling();
    }

    void UpdateFilling()
    {
        percentage =(float) (ph.currentHealth)/(ph.startingHealth);
        progressBar.fillAmount = percentage;
    }
}
