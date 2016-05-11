using UnityEngine;
using System.Collections;
using System.IO;

public class SkillLibraryBehavior : MonoBehaviour {
    public SkillLibrary skillLibrary;
    public TextAsset skillLibraryXml;
    StringReader xml;

    public UnityEngine.UI.Text debugText;

	// Use this for initialization
	void Start () {
        //skillLibrary.SketchInitialize();
        if (skillLibraryXml != null)
        {
            // TODO: fix xml reading after play testing
            //skillLibraryXml = (TextAsset)Resources.Load("SkillLibrary/SkillLibrary", typeof(TextAsset));
            xml = new StringReader(skillLibraryXml.text);
            //skillLibrary = SkillLibrary.Load(skillLibraryXml);
            skillLibrary = SkillLibrary.Load(xml);
        }
        else
        {
            // TODO: fix xml reading after play testing
            // skillLibrary = SkillLibrary.Load("SkillLibrary/SkillLibrary");
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            // skillLibrary.Save("./Assets/Resources/SkillLibrary/SkillLibrary.xml");
            //library.Save("MeleeSkillLibrary/MeleeSkillLibrary.xml");
        }
    }
}
