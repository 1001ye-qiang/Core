using System;

namespace GameCore.Buffer
{
    public interface IGameBuffer<TBufferType>
    {
        void AddBuffer(TBufferType bufferType, float time, long flag = 0L);

        void RefreshBuffer(TBufferType bufferType, float deltaTime);

        void RefreshAll(float deltaTime);

        bool IsContain(TBufferType bufferType);

        bool IsOver(TBufferType bufferType);

        void Clear(TBufferType bufferType);

        void Remove(TBufferType key);

        float GetRemainTime(TBufferType key);

        float GetAmount(TBufferType key);
    }
}
