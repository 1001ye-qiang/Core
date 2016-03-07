using UnityEngine;
using System.Collections;
using System;

public class ConnectManager : Singleton<ConnectManager>, IConnectManager
{
    public void addService(int id, IService service)
    {
        throw new NotImplementedException();
    }

    public IService getService(int id)
    {
        throw new NotImplementedException();
    }

    private ConnectManager()
    {
        connectPool = new System.Collections.Generic.Dictionary<uint, IConnect>();
    }

    public const uint dataCenterID = 1;//数据中心连接
    public const uint gameServerID = 2;//游戏服务器连接
    private System.Collections.Generic.Dictionary<uint, IConnect> connectPool;

    /// <summary>
    /// 注册连接
    /// </summary>
    public void regiestConnect(uint id, IConnect connect)
    {
        connectPool[id] = connect;
    }

    /// <summary>
    /// 指定id连接  发送阻塞消息
    /// </summary>
    public void sendBlock(IByteBuffer data, Response response, uint id = gameServerID)
    {
        if (connectPool.ContainsKey(id) == false)
        {
            return;
        }
        IConnect connect = connectPool[id];
        connect.sendBlock(data, response);
    }

    /// <summary>
    /// 指定id连接 发送消息
    /// </summary>
    public void send(IByteBuffer data, uint id = gameServerID)
    {
        if (connectPool.ContainsKey(id) == false)
        {
            return;
        }
        IConnect connect = connectPool[id];
        connect.send(data);
    }

    /// <summary>
    /// 关闭一个连接
    /// </summary>
    public void close(uint id)
    {
        if (connectPool.ContainsKey(id) == false)
        {
            return;
        }
        IConnect connect = connectPool[id];
        connect.close();
        connectPool.Remove(id);
    }


    private ArrayList list = new ArrayList();
    public void Update()
    {
        list.Clear();
        list.AddRange(connectPool.Values);
        foreach (IConnect item in list)
        {
            try
            {
                if (item != null)
                    item.update();
            }
            catch (Exception e)
            {
                Debug.LogError("Connect update error!");
            }
        }
    }
}
