using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Short : Weapon
{
    public Weapon_Short(string iconName) : base(iconName)
    {
    }

    public override Sprite GetIcon()
    {
        return ResourceManager.GetShort(strIconName);
    }
}
