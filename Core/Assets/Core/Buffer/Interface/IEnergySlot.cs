using System;

namespace GameCore.Buffer
{
    public interface IEnergySlot<TBufferType>
    {
        void AddBuffer(TBufferType bufferType, int maxBufferNum, float maxEnergyPerBuffer);

        void AddEnergy(TBufferType bufferType, float energy);

        int GetBufferNum(TBufferType bufferType);

        float GetBufferEnergy(TBufferType bufferType);

        void UseBuffer(TBufferType bufferType, int bufferNum);

        float GetEnergyAmount(TBufferType key);
    }
}
