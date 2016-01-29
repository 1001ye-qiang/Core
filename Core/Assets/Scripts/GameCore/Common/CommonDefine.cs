using UnityEngine;
using System.Collections;


public delegate void OnFinish(bool isFinish = true);


public struct Common
{
#region string 
    public const string g_3dRoot = "3D";
    public const string g_3dActorArea = "3D/ActorArea";
    public const string g_3dFactoryArea = "3D/FactoryArea";
    #region path def
    static Transform root;
    static Transform actorArea;
    static Transform factoryArea;

    public static Transform Root
    {
        get
        {
            if (root == null)
                root = CreateObjectTree(g_3dRoot);
            return root;
        }
    }
    public static Transform ActorArea
    {
        get
        {
            if (actorArea == null)
                actorArea = CreateObjectTree(g_3dActorArea);
            return actorArea;
        }
    }
    public static Transform FactoryArea
    {
        get
        {
            if (factoryArea == null)
                factoryArea = CreateObjectTree(g_3dFactoryArea);
            return factoryArea;
        }
    }


    static Transform CreateObjectTree(string name)
    {
        name = name.Replace('\\', '/');
        name.TrimEnd('/');
        GameObject obj = GameObject.Find(name);
        if(obj == null)
        {
            if (name.Contains("/"))
            {
                int index = name.LastIndexOf('/');
                Transform r = CreateObjectTree(name.Substring(0, index));
                obj = new GameObject(name.Substring(index + 1));
                obj.transform.parent = r;
            }
            else
            {
                obj = new GameObject(name);
            }
        }
        return obj.transform;
    }
    #endregion // path define



#endregion // string

}
