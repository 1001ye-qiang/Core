using UnityEngine;
using System.Collections;

public class SingleBase<T> where T : new()
{
    private static T inst;
    public static T Instance
    {
        get{
            if (inst == null)
                inst = new T();
            return inst;
        }
    }
}
