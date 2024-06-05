using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Long : Weapon
{
    public Weapon_Long(string iconName) : base(iconName)
    {
    }

    public override Sprite GetIcon()
    {
        return ResourceManager.GetLong(strIconName);
    }
}
