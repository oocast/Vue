using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VoteTimerBar : MonoBehaviour {
    TimedVoting timedVoting;
    Image progressBar;
    
    float waitTime;
    float percentage;
    float switchTime;
    bool countdown;

    void Awake()
    {
        timedVoting = GameObject.Find("Twitch Vote").GetComponent<TimedVoting>();
        if (timedVoting != null)
        {
            timedVoting.onVoteSwitch += ResetTimer;
            switchTime = timedVoting.switchTime;
        }
    }

	// Use this for initialization
	void Start () {
        waitTime = 0f;
        percentage = 0f;
        progressBar = GetComponent<Image>();
        progressBar.fillAmount = 0f;
        countdown = false;
    }
	
	// Update is called once per frame
	void Update () {
        UpdateFilling();
    }

    void ResetTimer(Vote[] votes)
    {
        waitTime = votes[1].duration;
        percentage = 1f;
        // progressBar.fillAmount = percentage;
        countdown = false;
        StartCoroutine(StartCountDown());
    }

    void UpdateFilling()
    {
        if (waitTime > 0f && countdown)
        {
            percentage -= Time.deltaTime / waitTime;
            progressBar.fillAmount = percentage;
        }

    }

    IEnumerator StartCountDown()
    {
        yield return new WaitForSeconds(switchTime);
        countdown = true;
        Debug.Log("Start countdown!");
    }
}
