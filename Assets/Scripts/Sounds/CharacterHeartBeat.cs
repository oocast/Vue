using UnityEngine;
using System.Collections;

public class CharacterHeartBeat : MonoBehaviour {
    private float _frequency;
    private bool _soundPlaying;
    private float _prevPlayTime;

	// Use this for initialization
	void Start () {
        _frequency = 0f;
        _soundPlaying = false;
        _prevPlayTime = 0f;
    }
	
	// Update is called once per frame
	void Update () {
	    if (_soundPlaying)
        {
            float interval = 1f / _frequency;
            if (Time.time > interval + _prevPlayTime)
            {
                _prevPlayTime = Time.time;
                GetComponent<SoundSystem>().PlaySound(Camera.main.transform.position, "Heartbeat");
            }
        }
	}

    public void UpdateFrequency(float processToDeath)
    {
        /*if (processToDeath > 0.95f)
        {
            ResetFrequency();
        }
        else */if (processToDeath > 0.005f)
        {
            _frequency = processToDeath * 2f;
            _soundPlaying = true;
        }
        else
        {
            ResetFrequency();
        }
    }

    public void ResetFrequency()
    {
        _frequency = 0f;
        _soundPlaying = false;
    }
}
