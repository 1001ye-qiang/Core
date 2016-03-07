using UnityEngine;
using System.Collections;

// sender 定义一堆函数，界面引用这些。
//或者更好的是，有一个中间件，把界面的event和这里的函数关联起来
/// <summary>
/// 登录模块，发送逻辑
/// </summary>
public class LoginNetSender: Singleton<LoginNetSender> {

    public delegate void EnableLoading();
    public delegate void DisableLoading();

    public delegate void Send(Dictionary dic);
    public delegate void SendBlock(Dictionary dic);
    public delegate void Receive(Dictionary dic);


    public void ConnetDC(Dictionary dic)
    {
        //getConnect().regiestConnect(GameConnectServer.dataCenterId, new GameDataConnect());
        //uint id = GameConnectServer.dataCenterId;
        //string ip = (string)dic["ip"];
        //int port = (int)dic["port"];
        //getConnect().connectToServer(id, ip, port);

    }

    public void SendLogin(Dictionary dic)
    {
        IByteBuffer sendData = ByteBufferManager.Instance.GetObj();

    }


}
