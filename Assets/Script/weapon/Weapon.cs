using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    public override Sprite GetIcon()
    {
        return ResourceMgr.GetWeapon(strIconName);
    }
}
