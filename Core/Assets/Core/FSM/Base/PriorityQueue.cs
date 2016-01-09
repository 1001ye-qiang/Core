using System;
using System.Collections.Generic;

namespace GameCore.FSM
{
    public class PriorityQueue<T> where T : IComparable
    {
        protected List<T> mList;

        public PriorityQueue()
        {
            this.mList = new List<T>();
        }

        public bool Enqueue(T element)
        {
            if (!this.mList.Contains(element))
            {
                this.mList.Add(element);
                this.mList.Sort();
                return true;
            }
            return false;
        }

        public T Dequeue()
        {
            T t = this.mList[0];
            this.mList.Remove(t);
            return t;
        }

        public T Peek()
        {
            return this.mList[0];
        }

        public bool Empty()
        {
            return this.mList.Count == 0;
        }
    }
}
