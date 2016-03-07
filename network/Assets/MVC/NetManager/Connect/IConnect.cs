using UnityEngine;
using System.Collections;

public interface IConnect
{
    void connectionOpened(INetConnect netconnection);

    void connectionClosed(INetConnect netconnection);

    void connectionError(INetConnect netconnection, string msg);

    void messageArrived(INetConnect netconnection, IByteBuffer data);

    void connectToServer(string host, int port);

    void close();

    void sendBlock(IByteBuffer data, Response response);

    void send(IByteBuffer data);

    //void setCodeKit(ICodeKit c);

    void update();
}
