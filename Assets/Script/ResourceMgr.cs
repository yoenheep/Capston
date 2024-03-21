using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMgr : MonoBehaviour
{
    [SerializeField] private List<Sprite> listWeapons;
    private static Dictionary<string, Sprite> dicWeapons;

    private void Awake()
    {
        dicWeapons = new Dictionary<string, Sprite>();

        foreach (Sprite sp in listWeapons)
        {
            dicWeapons.Add(MakeName(sp.name), sp);
        }
    }

    private static string MakeName(string str)
    {
        return str.ToLower().Replace(" ", "");
    }

    public static Sprite GetWeapon(string name)
    {
        name = MakeName(name);
        if (!dicWeapons.TryGetValue(name, out Sprite sp))
            return null;

        return sp;
    }
}
