using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class OptionDictionary : MonoBehaviour {
    [System.Serializable]
    public class StringPair
    {
        public string option;
        public string voteResourceFileName;
    }

    public StringPair[] dictionaryEntries;
    public StringPair[] prefabNameEntries;
    Dictionary<string, string> dictionary;
    Dictionary<string, string> prefabNameDictionary;

    // Use this for initialization
    void Start () {
        dictionary = new Dictionary<string, string>();
        for (int i = 0; i < dictionaryEntries.Length; i++)
        {
            dictionary.Add(dictionaryEntries[i].option, dictionaryEntries[i].voteResourceFileName);
        }

        prefabNameDictionary = new Dictionary<string, string>();
        for (int i = 0; i < prefabNameEntries.Length; i++)
        {
            prefabNameDictionary.Add(prefabNameEntries[i].option, prefabNameEntries[i].voteResourceFileName);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public string GetVoteResourceFileName(string option)
    {
        string voteResourceFileName = null;
        if (dictionary.ContainsKey(option))
        {
            voteResourceFileName = dictionary[option];
        }
        return voteResourceFileName;
    }

    public string GetPrefabResourceFileName(string option)
    {
        string prefabResourceFileName = null;
        if (prefabNameDictionary.ContainsKey(option))
        {
            prefabResourceFileName = prefabNameDictionary[option];
        }
        return prefabResourceFileName;
    }
}
