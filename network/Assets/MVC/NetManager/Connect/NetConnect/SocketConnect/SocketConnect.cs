using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;


public class Eventer
{
    private Queue<SocketConnect.ClientEvent> events;
    private SocketConnect socket;
    public Eventer(SocketConnect s)
    {
        socket = s;
        events = new Queue<SocketConnect.ClientEvent>();
    }

    public void AddEvent(SocketConnect.ClientEvent e)
    {
        if (e != null)
        {
            lock (this.events)
            {
                this.events.Enqueue(e);
            }
        }
    }

    public void update()
    {
        // 处理事件队列
        lock (this.events)
        {
            for (; this.events.Count > 0;)
            { 
                try
                {
                    this.events.Peek()(socket);
                }
                finally
                {
                    this.events.Dequeue();
                }
            }
        }
    }
}

public class SocketConnect : ISocketConnect
{
    private string address;
    private int port;
    private Socket socket;
    private bool conned;

    private byte[] buffer;
    private int recvSize;
    private IByteBuffer bytesRecved;

    private AsyncCallback conCallBack;
    private AsyncCallback readCallback;
    private AsyncCallback sendCallback;

    /// <summary>
    /// 消息发送队列
    /// </summary>
    private LinkedList<IByteBuffer> transfer;
    /// <summary>
    /// 消息接收队列
    /// </summary>
    private LinkedList<IByteBuffer> receives;


    ///// <summary>
    ///// 协议编/解码
    ///// </summary>
    //protected IProtocol protocol;
    ///// <summary>
    ///// 接收消息处理服务
    ///// </summary>
    //protected IService service;

    private bool sendAsyncResult = false;
    private IAsyncResult connectAsyncResult;
    protected Exception exception;

    #region Event for Interface
    private Eventer events;
    public delegate void ClientEvent(SocketConnect sock);
    public delegate void OnReceiveData(IByteBuffer data);

    private ClientEvent connectedEvent;
    private ClientEvent errorEvent;
    private ClientEvent disconnectEvent;
    private ClientEvent receivedEvent;
    private OnReceiveData onReceive;

    public ClientEvent ConnectedEvent
    {
        get
        {
            return connectedEvent;
        }

        set
        {
            connectedEvent = value;
        }
    }

    public ClientEvent ErrorEvent
    {
        get
        {
            return errorEvent;
        }

        set
        {
            errorEvent = value;
        }
    }

    public ClientEvent DisconnectEvent
    {
        get
        {
            return disconnectEvent;
        }

        set
        {
            disconnectEvent = value;
        }
    }

    public ClientEvent ReceivedEvent
    {
        get
        {
            return receivedEvent;
        }

        set
        {
            receivedEvent = value;
        }
    }

    public OnReceiveData OnReceive
    {
        get
        {
            return onReceive;
        }

        set
        {
            onReceive = value;
        }
    }
    #endregion // Event

    public SocketConnect()
    {
        this.buffer = new byte[1024];
        bytesRecved = new ByteBuffer();
        transfer = new LinkedList<IByteBuffer>();
        receives = new LinkedList<IByteBuffer>();

        //this.events = new LinkedList<ClientEvent>();
        this.conCallBack = new AsyncCallback(ConnectCallBack);
        this.readCallback = new AsyncCallback(ReadCallback);
        this.sendCallback = new AsyncCallback(SendCallback);

        events = new Eventer(this);
    }

    #region Connect and Callback
    public void Connect(String address, int port)
    {
        if (socket != null && socket.Connected)
        {
            throw new Exception("Already connect to " + socket.RemoteEndPoint.ToString());
        }
        if (connectAsyncResult != null)
        {
            throw new Exception("In process of connect... " + socket.RemoteEndPoint.ToString());
        }
        if (socket != null)
        {
            socket.Close();
        }
        this.address = address;
        this.port = port;
        IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(address), port);
        this.socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        connectAsyncResult = this.socket.BeginConnect(ipe, conCallBack, this);

    }

    private void ConnectCallBack(IAsyncResult result)
    {
        SocketConnect client = (SocketConnect)result.AsyncState;
        if (client == this)
        {
            try
            {
                socket.EndConnect(result);

                this.conned = socket.Connected;
                if (this.conned)
                {
                    // 准备读取
                    BeginRead();
                    events.AddEvent(ConnectedEvent);
                }
            }
            catch (Exception e)
            {
                exception = e;
                events.AddEvent(ErrorEvent);
            }
            connectAsyncResult = null;
        }
    }
    
    #endregion // connect

    #region Read and Callback
    private void BeginRead()
    {
        socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, readCallback, this);
    }
    protected void ReadCallback(IAsyncResult result)
    {
        SocketConnect client = (SocketConnect)result.AsyncState;
        if (client == this && client.socket.Connected)
        {
            recvSize = socket.EndReceive(result);
            if (recvSize == 0)
            {
                if (conned)
                {
                    this.conned = false;
                    this.socket.Shutdown(SocketShutdown.Both);
                    this.socket.Close();
                    events.AddEvent(DisconnectEvent);
                }
            }
            else
            {
                if (recvSize > 0)
                {
                    // 有数据到达需要接收
                    onRead(recvSize);
                    recvSize = 0;

                    // 继续异步读取数据
                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, readCallback, this);

                    // 触发接收事件 now not used, we use update
                    events.AddEvent(ReceivedEvent);
                }
            }
        }
    }

    /// <summary>
    /// 数据到达
    /// </summary>
    /// <param name="size"></param>
    protected void onRead(int size)
    {
        // 将接收到的数据写入缓冲区
        bytesRecved.writeBytes(buffer, 0, size);

        if (bytesRecved.available() > 0)
        {
            while (decode(bytesRecved))
            {
                // 收到一个消息，等待Update处理
            }
        }
    }

    /// <summary>
    /// 把数据划分成一个一个的包，以便上层解析
    /// 因为低是一个池子，所有的消息可能全黏在一起
    /// </summary>
    /// <param name="bytebuffer"></param>
    /// <returns></returns>
    public bool decode(IByteBuffer bytebuffer) 
    {
        if (bytebuffer.available() < 4)
            return false;
        int i = bytebuffer.getReadPos();
        int len = bytebuffer.readInt();
        if (len == 1014001516)
        {
            return true;
        }
        if (bytebuffer.available() < len)
        {
            bytebuffer.setReadPos(i);
            return false;
        }
        else
        {
            IByteBuffer message = ByteBufferManager.Instance.GetObj();
            message.writeByteBuffer(bytebuffer, len);
            bytebuffer.pack();
            recv(message);
            return true;
        }
    }
    /// <summary>
    /// 收到协议解码器通知，解码出一条消息。
    /// </summary>
    /// <param name="msg">收到的消息</param>
    public void recv(IByteBuffer msg)
    {
        lock (receives)
        {
            receives.AddLast(msg);
        }
    }
    #endregion // read

    #region Send and Callback
    public void send(IByteBuffer data)
    {
        if (conned == false)
        {
            return;
        }

        // 推送消息到服务端
        lock (this.transfer)
        {
            this.transfer.AddLast(data);
            BeginSend();
        }
    }
    private void BeginSend()
    {
        if (conned && !sendAsyncResult)
        {
            IByteBuffer data = this.transfer.First.Value;
            this.sendAsyncResult = true;
            if (socket.Connected)
            {
                socket.BeginSend(data.getRawBytes(), data.getReadPos(), data.available(), SocketFlags.None, sendCallback, this);
            }
            else
            {
                //断线
                conned = false;
                socket.Close();
                //发送断线消息
                //Facade.GetInstance(GameFacade.OPTION_FACADE).SendNotification(OptionFacadeCMD.SHOW_SYSTEM_OPTION, "与服务器断开连接");
            }
        }
    }

    private void SendCallback(IAsyncResult result)
    {
        SocketConnect client = (SocketConnect)result.AsyncState;
        if (client == this && client.socket.Connected)
        {
            try
            {
                IByteBuffer data;
                lock (this.transfer)
                {
                    data = this.transfer.First.Value;
                    int sendsize = client.socket.EndSend(result);

                    if (sendsize > 0)
                    {
                        int lastsize = data.available();
                        this.sendAsyncResult = false;
                        if (sendsize >= lastsize)
                        {
                            // 移除头并发送下一个
                            data.setReadPos(data.getReadPos() + sendsize);
                            this.transfer.RemoveFirst();
                            if (this.transfer.Count > 0)
                            {
                                BeginSend();
                                return;
                            }
                        }
                        else
                        {
                            // 继续发送剩余字节
                            data.setReadPos(data.getReadPos() + sendsize);
                            BeginSend();
                            return;
                        }
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
        else
        {
        }
    }
    
    #endregion // send

    public void update()
    {
        if (socket == null)
            return;

        // 处理事件队列
        events.update();
        // 处理服务器消息
        lock (this.receives)
        {
            for (; conned && this.receives.Count > 0;)
                try
                {
                    if(null != OnReceive) OnReceive(this.receives.First.Value);
                }
                finally
                {
                    this.receives.RemoveFirst();
                }
        }
    }

    public void Close()
    {
        if (this.socket != null && this.socket.Connected)
        {
            socket.Close();
        }
    }

    public string getAddress()
    {
        return address;
    }

    public int getPort()
    {
        return port;
    }

    public bool isClosed()
    {
        return !conned;
    }
}
