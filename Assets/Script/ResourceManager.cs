using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> listShorts;
    private static Dictionary<string, Sprite> dictShorts;

    [SerializeField] private List<Sprite> listLongs;
    private static Dictionary<string, Sprite> dictLongs;

    private void Awake()
    {
        dictShorts = new Dictionary<string, Sprite>();
        dictLongs = new Dictionary<string, Sprite>();

        foreach(Sprite sp in listShorts)
        {
            dictShorts.Add(MakeName(sp.name), sp);
        }

        foreach(Sprite sp in listLongs)
        {
            dictLongs.Add(MakeName(sp.name), sp);
        }
    }

    private static string MakeName(string str)
    {
        return str.ToLower().Replace(" ", "");
    }

    public static Sprite GetShort(string name)
    {
        name = MakeName(name);
        if (!dictShorts.TryGetValue(name, out Sprite sp))
            return null;

        return sp;
    }

    public static Sprite GetLong(string name)
    {
        name = MakeName(name);
        if (!dictLongs.TryGetValue(name, out Sprite sp))
            return null;

        return sp;
    }
}
