using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class SoundSystem : MonoBehaviour
{
    [System.Serializable]
    public class ClipEntry
    {
        public string name;
        public AudioClip clip;
    }

    public ClipEntry[] clips;
    Dictionary<string, int> lookupTable;
    string currentBGM;

    // Use this for initialization
    void Start()
    {
        lookupTable = new Dictionary<string, int>();
        for (int i = 0; i < clips.Length; i++)
        {
            lookupTable.Add(clips[i].name, i);
        }
        currentBGM = "Chill";
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlaySound(Vector3 position, string clipName, float volume = 1f)
    {
        if (lookupTable.ContainsKey(clipName))
        {
            GameObject soundSource = new GameObject("Sound Source", typeof(AudioSource));// Instantiate(soundSourcePrefab, position, Quaternion.identity) as GameObject;
            soundSource.transform.position = position;
            AudioSource source = soundSource.GetComponent<AudioSource>();
            source.clip = clips[lookupTable[clipName]].clip;
            source.volume = volume;
            source.Play();
            Destroy(soundSource, source.clip.length + 1f);
        }
    }

    public GameObject PlayLoopingSound(Vector3 position, string clipName, float volume = 1f)
    {
        if (lookupTable.ContainsKey(clipName))
        {
            GameObject soundSource = new GameObject("Sound Source", typeof(AudioSource));// Instantiate(soundSourcePrefab, position, Quaternion.identity) as GameObject;
            soundSource.transform.position = position;
            AudioSource source = soundSource.GetComponent<AudioSource>();
            source.clip = clips[lookupTable[clipName]].clip;
            source.volume = volume;
            source.loop = true;
            source.Play();
            return soundSource;
        }
        else
        {
            return null;
        }
    }

    public AudioClip GetSoundClip(string clipName)
    {
        if (lookupTable.ContainsKey(clipName))
        {
            return clips[lookupTable[clipName]].clip;
        }
        else
        {
            return null;
        }
    }

    public void ChangeBGM(string clipName)
    {
        if (lookupTable.ContainsKey(clipName) && !currentBGM.Equals(clipName))
        {
            StartCoroutine(ChangeBGMCoroutine(clipName));
            currentBGM = clipName;
        }
    }

    IEnumerator ChangeBGMCoroutine(string clipName)
    {
        AudioSource source = GameObject.Find("Main Camera/BGM").GetComponent<AudioSource>();
        source.DOFade(0f, 1f);
        yield return new WaitForSeconds(1f);
        source.Stop();
        source.clip = clips[lookupTable[clipName]].clip;
        source.Play();
        source.DOFade(1f, 1f);
    }
}
