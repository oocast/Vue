using UnityEngine;
using System.Collections;
using System.IO;

public class MeleeSkillLibraryBehavior : MonoBehaviour {
    public MeleeSkillLibrary library;
    public TextAsset meleeSkillLibraryXml;

    void Awake()
    {
        // TODO: read from JSON file for skill definations
        // TODO: or read the skill only when it's needed
    }

    void Start()
    {
        if (meleeSkillLibraryXml != null)
        {
            // TODO: read XML
            // library = MeleeSkillLibrary.Load(meleeSkillLibraryXml);
            StringReader stringReader = new StringReader(meleeSkillLibraryXml.text);
            library = MeleeSkillLibrary.Load(stringReader);
        }
        else
        {
            // TODO: read XML
            // library = MeleeSkillLibrary.Load("MeleeSkillLibrary/MeleeSkillLibrary");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //library.Save("./Assets/Resources/MeleeSkillLibrary/MeleeSkillLibrary.xml");
            //library.Save("MeleeSkillLibrary/MeleeSkillLibrary.xml");
        }
    }
}
