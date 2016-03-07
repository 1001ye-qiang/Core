using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Connect : IConnect
{
    private Dictionary<Int32, Response> responses = new Dictionary<int, Response>();
    private NetConnect con;
    
    protected Connect()
    {
    }

    public void setCodeKitValue(ICodeKit codeKey)
    {
        con.CodeKit = codeKey;
    }

    public void processMessage(IByteBuffer data)
    {
        if (data.available() <= 0)
        {
            return;
        }
        int id = data.readByte();
        if (id == 0)
        {
            onBlock(data);
        }
        else
        {
            data.setReadPos(data.getReadPos() - 1);
            //messageArrived(data);
        }
    }

    private void onBlock(IByteBuffer data)
    {
        int sendId = data.readInt();

        if (this.responses.ContainsKey(sendId) == false)
        {
            return;
        }
        Response res = this.responses[sendId];
        // 移除
        this.responses.Remove(sendId);

        // 处理响应
        int typeId = data.readShort();
        //if (typeId == 200)
        //{
        //    res.onSuccess(data);
        //}
        //else
        //{
        //    data.setReadPos(data.getReadPos() - 2);
        //    res.onFault(data);
        //}
    }

    public void sendBlock(IByteBuffer data, Response response)
    {
        int sendId = getNextId();
        IByteBuffer message = ByteBufferManager.Instance.GetObj();
        int pos = data.getReadPos();
        message.writeByte(data.readByte());
        message.writeInt(sendId);
        message.writeByteBuffer(data, data.available());
        data.setReadPos(pos);
        responses[sendId] = response;
        con.send(message);
    }

    private int blockId = 1000;
    private int getNextId()
    {
        return blockId++;
    }

    public void connectionOpened(INetConnect netconnection)
    {
        throw new NotImplementedException();
    }

    public void connectionClosed(INetConnect netconnection)
    {
        throw new NotImplementedException();
    }

    public void connectionError(INetConnect netconnection, string msg)
    {
        throw new NotImplementedException();
    }

    public void messageArrived(INetConnect netconnection, IByteBuffer data)
    {
        throw new NotImplementedException();
    }

    public void connectToServer(string host, int port)
    {
        throw new NotImplementedException();
    }

    public void close()
    {
        throw new NotImplementedException();
    }

    public void send(IByteBuffer data)
    {
        throw new NotImplementedException();
    }

    public void update()
    {
        throw new NotImplementedException();
    }
}
