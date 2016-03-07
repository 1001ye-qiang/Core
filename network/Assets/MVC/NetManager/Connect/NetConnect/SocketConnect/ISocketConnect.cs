public interface ISocketConnect{

    /// <summary>
    ///  发出数据 
    /// </summary>
    void send(IByteBuffer data);

    /// <summary>
    /// 收到消息
    /// </summary>
    /// <param name="data"></param>
    void recv(IByteBuffer data);

    /// <summary>
    ///  关闭连接 
    /// </summary>
    void Close();

    /// <summary>
    ///  是否已经关闭 
    /// </summary>
    bool isClosed();

    /// <summary>
    ///  得到连接的地址 
    /// </summary>
    string getAddress();

    /// <summary>
    ///  得到端口 
    /// </summary>
    int getPort();
}
