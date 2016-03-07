using UnityEngine;
using System.Collections;

public class LoginNetReceiver : Singleton<LoginNetReceiver>
{

    public delegate void EnableLoading();
    public delegate void DisableLoading();

    public delegate void Send(Dictionary dic);
    public delegate void SendBlock(Dictionary dic);
    public delegate void Receive(Dictionary dic);

    public void SendLogin(Dictionary dic)
    {
    }


}
