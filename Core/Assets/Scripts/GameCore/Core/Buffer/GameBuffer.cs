using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Buffer
{
    public class GameBuffer<TBufferType> : IGameBuffer<TBufferType>
    {
        public class BufferData
        {
            public float remainTime;

            public float maxTime;

            public long flag;

            public BufferData(float time, long flag)
            {
                this.Reset(time, flag);
            }

            public void Reset(float time, long flag)
            {
                this.remainTime = time;
                this.maxTime = time;
                this.flag = flag;
            }

            public void Reset(float time)
            {
                this.remainTime = time;
                this.maxTime = time;
            }

            public void Clear()
            {
                this.remainTime = 0f;
            }

            public float GetAmount()
            {
                return 1f - Mathf.Max(this.remainTime, 0f) / this.maxTime;
            }
        }

        protected Dictionary<TBufferType, GameBuffer<TBufferType>.BufferData> mBuffer = new Dictionary<TBufferType, GameBuffer<TBufferType>.BufferData>();

        public virtual void AddBuffer(TBufferType bufferType, float time, long flag = 0L)
        {
            if (this.mBuffer.ContainsKey(bufferType))
            {
                this.mBuffer[bufferType].Reset(time, flag);
                return;
            }
            this.mBuffer.Add(bufferType, new GameBuffer<TBufferType>.BufferData(time, flag));
        }

        public void RefreshBuffer(TBufferType bufferType, float deltaTime)
        {
            if (this.mBuffer.ContainsKey(bufferType))
            {
                this.mBuffer[bufferType].remainTime -= deltaTime;
            }
        }

        public void RefreshAll(float deltaTime)
        {
            foreach (KeyValuePair<TBufferType, GameBuffer<TBufferType>.BufferData> current in this.mBuffer)
            {
                current.Value.remainTime -= deltaTime;
            }
        }

        public bool IsContain(TBufferType bufferType)
        {
            return this.mBuffer.ContainsKey(bufferType);
        }

        public bool IsOver(TBufferType bufferType)
        {
            return this.mBuffer[bufferType].remainTime <= 0.01f;
        }

        public void Clear(TBufferType key)
        {
            this.mBuffer[key].remainTime = 0f;
        }

        public void Remove(TBufferType key)
        {
            this.mBuffer.Remove(key);
        }

        public float GetRemainTime(TBufferType key)
        {
            if (this.mBuffer.ContainsKey(key))
            {
                return this.mBuffer[key].remainTime;
            }
            return 0f;
        }

        public float GetAmount(TBufferType key)
        {
            if (this.mBuffer.ContainsKey(key))
            {
                return this.mBuffer[key].GetAmount();
            }
            return 0f;
        }
    }
}
