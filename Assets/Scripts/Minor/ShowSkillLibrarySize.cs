using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowSkillLibrarySize : MonoBehaviour {
    public SkillLibraryBehavior libBehavior;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Text>().text = libBehavior.skillLibrary.skills.Length.ToString();

    }
}
