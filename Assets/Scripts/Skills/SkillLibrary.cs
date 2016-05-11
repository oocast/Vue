using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("SkillLibrary")]
[System.Serializable]
public class SkillLibrary {
    [XmlArray("Skills")]
    [XmlArrayItem("Skill", typeof(Skill))]
    [XmlArrayItem("ShockWaveSkill", typeof(ShockWaveSkill))]
    [XmlArrayItem("AOESkill", typeof(AOESkill))]
    [XmlArrayItem("BombSkill", typeof(BombSkill))]
    [XmlArrayItem("MovementSkill", typeof(MovementSkill))]
    [XmlArrayItem("BladestormSkill", typeof(BladestormSkill))]
    public Skill[] skills;

    public MovementSkill blinkSkill;

    public void SketchInitialize()
    {
        skills = new Skill[5];
        skills[0] = new Skill();
        skills[1] = new ShockWaveSkill();
        skills[2] = new AOESkill();
        skills[3] = new BombSkill();
        skills[4] = new MovementSkill();
    }

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(SkillLibrary));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static SkillLibrary Load(string path)
    {
        var serializer = new XmlSerializer(typeof(SkillLibrary));
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        using (var stream = new MemoryStream(textAsset.bytes))
        {
            return serializer.Deserialize(stream) as SkillLibrary;
        }
    }

    public static SkillLibrary Load(TextAsset textAsset)
    {
        var serializer = new XmlSerializer(typeof(SkillLibrary));
        using (var stream = new MemoryStream(textAsset.bytes))
        {
            return serializer.Deserialize(stream) as SkillLibrary;
        }
    }


    public static SkillLibrary Load(StringReader xml)
    {
        var serializer = new XmlSerializer(typeof(SkillLibrary));
        return serializer.Deserialize(xml) as SkillLibrary;
    }

    public static SkillLibrary Load(TextAsset textAsset, UnityEngine.UI.Text debugText)
    {
        var serializer = new XmlSerializer(typeof(SkillLibrary));
        SkillLibrary result;
        using (var stream = new MemoryStream(textAsset.bytes))
        {
            debugText.text = "Start deserialize";
            result =  serializer.Deserialize(stream) as SkillLibrary;
            debugText.text = "End deserialize";
        }
        return result;
    }
}
