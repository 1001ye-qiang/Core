using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// reveiver 定义一堆事件，界面实现函数。
// 有一个中间件，把 界面的函数 和 receiver的event 关联起来
public class ReceiverManager : Singleton<ReceiverManager>
{
    ReceiverManager()
    {
        recvMap = new Dictionary<int, CallBack>();
    }
    public Dictionary<int, CallBack> recvMap;

    public delegate void CallBack(Dictionary dic); // event

    public void Register(int id, CallBack cb)
    {
        if (recvMap.ContainsKey(id))
        {
            recvMap[id] += cb;
        }
        else
        {
            recvMap.Add(id, cb);
        }
    }
}

