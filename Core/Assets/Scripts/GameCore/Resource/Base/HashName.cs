
using System.Collections.Generic;
using UnityEngine;

public class HashName
{
    public class HashNameNode
    {
        public HashName mHashName;

        public HashName.HashNameNode mNext;

        public HashNameNode()
        {
            this.mHashName = null;
            this.mNext = null;
        }
    }

    protected int mId;

    public static int msHashTableLength = 2048;

    protected static HashName.HashNameNode[] msHashNameNodes = new HashName.HashNameNode[HashName.msHashTableLength];

    protected static List<string> msNameTable = new List<string>();

    public static int BKDRHash(string str)
    {
        int num = 131;
        int num2 = 0;
        char[] array = str.ToCharArray();
        for (int i = 0; i < array.Length; i++)
        {
            num2 = num2 * num + (int)array[i];
        }
        return num2 & 2147483647;
    }

    public static HashName Cteate(string name)
    {
        return new HashName(name);
    }

    public HashName(string name)
    {
        this.mId = this.GenerateId(name);
    }

    public int GetId()
    {
        return this.mId;
    }

    public string GetName()
    {
        return HashName.ToName(this.GetId());
    }

    public static string ToName(int id)
    {
        return HashName.msNameTable[id];
    }

    protected int GenerateId(string name)
    {
        int num = HashName.BKDRHash(name) % HashName.msHashTableLength;
        if (num < 0)
        {
            num = HashName.msHashTableLength + num;
        }
        if (HashName.msHashNameNodes[num] == null)
        {
            HashName.msHashNameNodes[num] = new HashName.HashNameNode();
        }
        HashName.HashNameNode hashNameNode = HashName.msHashNameNodes[num];
        if (hashNameNode.mHashName == null)
        {
            int result = this.CreateNewId(name);
            hashNameNode.mHashName = this;
            return result;
        }
        while (!(HashName.msNameTable[hashNameNode.mHashName.GetId()] == name))
        {
            if (hashNameNode.mNext == null)
            {
                hashNameNode.mNext = new HashName.HashNameNode();
                int result2 = this.CreateNewId(name);
                hashNameNode.mNext.mHashName = this;
                return result2;
            }
            hashNameNode = hashNameNode.mNext;
            if (hashNameNode == null)
            {
                return -1;
            }
        }
        return hashNameNode.mHashName.GetId();
    }

    protected int CreateNewId(string name)
    {
        int count = HashName.msNameTable.Count;
        HashName.msNameTable.Add(name);
        return count;
    }
}