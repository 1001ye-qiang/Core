using UnityEngine;
using System.Collections;

public interface INetConnect {

    void send(IByteBuffer data);
    void recv(IByteBuffer data);
}
