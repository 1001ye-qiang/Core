using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

// 这个类用来保证连接的
public class NetConnect : INetConnect
{
    public static readonly string DEFAULT_CODEKEY = System.Text.Encoding.Default.GetString(new byte[] { 99, 104, 114, 100, 119 });
    private ICodeKit codeKit = new DefaultCodeKit(DEFAULT_CODEKEY);

    public ICodeKit CodeKit
    {
        get { return this.codeKit; }
        set { this.codeKit = value; }
    }

    SocketConnect sc;
    public SocketConnect Sc
    {
        get { return sc; }
    }

    public NetConnect()
    {
    }

    public void recv(IByteBuffer data)
    {
        throw new NotImplementedException();
    }

    public void send(IByteBuffer data)
    {
        // 创建消息，并发送
        int j = data != null ? data.available() : 0;
        IByteBuffer message = ByteBufferManager.Instance.GetObj();
        message.writeInt(j);
        if (data != null && j > 0)
        {
            int offset = message.getWritePos();
            int k = data.getReadPos();
            message.writeByteBuffer(data, j);
            data.setReadPos(k);
            // 加密消息内容
            codeKit.coding(message, offset + 1, j - 1);
        }
        sc.send(message);
    }
}
