using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    protected string strIconName;
    public Weapon(string iconName)
    {
        this.strIconName = iconName;
    }
    public abstract Sprite GetIcon();
}
