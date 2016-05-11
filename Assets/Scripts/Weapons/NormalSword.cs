using UnityEngine;
using System.Collections;

public class NormalSword : Weapon
{
    void Initialize()
    {
        base.Initialize();
    }

    public override void DebugLog()
    {
        Debug.Log("NormalSword");
    }

}
