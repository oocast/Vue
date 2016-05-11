using UnityEngine;
using System.Collections;

public class MusicVote : GameContentVote {
    SoundSystem soundSystem;

    // Use this for initialization
    void Start()
    {
        GameObject soundSystemObj = GameObject.Find("Sound System");
        if (soundSystemObj != null)
        {
            soundSystem = soundSystemObj.GetComponent<SoundSystem>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void ChangeContent(params string[] content)
    {
        if (content.Length > 0)
        {
            soundSystem.ChangeBGM(content[0]);
        }
    }
}
