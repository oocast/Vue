using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("MeleeSkillLibrary")]
[System.Serializable]
public class MeleeSkillLibrary {
    [XmlArray("MeleeSkills"),XmlArrayItem("MeleeSkill")]
    public MeleeSkill[] skills;

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(MeleeSkillLibrary));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static MeleeSkillLibrary Load(string path)
    {
        var serializer = new XmlSerializer(typeof(MeleeSkillLibrary));
        TextAsset textAsset = Resources.Load(path) as TextAsset;
        using (var stream = new MemoryStream(textAsset.bytes))
        {
            return serializer.Deserialize(stream) as MeleeSkillLibrary;
        }
    }

    public static MeleeSkillLibrary Load(StringReader xml)
    {
        var serializer = new XmlSerializer(typeof(MeleeSkillLibrary));
        return serializer.Deserialize(xml) as MeleeSkillLibrary;
    }

    public static MeleeSkillLibrary Load(TextAsset textAsset)
    {
        var serializer = new XmlSerializer(typeof(MeleeSkillLibrary));
        using (var stream = new MemoryStream(textAsset.bytes))
        {
            return serializer.Deserialize(stream) as MeleeSkillLibrary;
        }
    }
}
