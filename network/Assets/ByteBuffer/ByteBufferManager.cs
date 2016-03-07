using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ByteBufferManager : Singleton<ByteBufferManager>
{
    public Queue<IByteBuffer> bufPool;

    private ByteBufferManager()
    {
        bufPool = new Queue<IByteBuffer>();
    }

    public IByteBuffer GetObj()
    {
        IByteBuffer classObj;
        if (bufPool.Count > 0)
        {
            classObj = bufPool.Dequeue();
            return classObj;
        }
        classObj = new ByteBuffer();
        return classObj;
    }

    public void Recycle(IByteBuffer buf)
    {
        buf.Clear();
        //buf.getRawBytes() = null;
    }
    public void Clear()
    {
        if (bufPool.Count > 0)
        {
            IByteBuffer classObj;
            do
            {
                classObj = bufPool.Dequeue();
                classObj.Clear();
                classObj = null;
            }
            while (bufPool.Count > 0);
            bufPool.Clear();
        }
        bufPool = null;
    }
}
