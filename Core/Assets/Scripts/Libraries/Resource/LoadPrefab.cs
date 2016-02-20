using UnityEngine;
using System.Collections;

public class LoadPrefab {
    public static GameObject LoadResource(string path)
    {
        return GameObject.Instantiate(Resources.Load(path)) as GameObject;
    }
}
