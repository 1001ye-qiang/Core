using System;
using System.Collections.Generic;

namespace GameCore.Buffer
{
    public class EnergySlot<TBufferType> : IEnergySlot<TBufferType>
    {
        public class SlotData
        {
            public float currentEnergy;

            public int currentBufferNum;

            public int maxBufferNum;

            public float maxEnergyPerBuffer;

            public SlotData(int maxBufferNum, float maxEnergyPerBuffer)
            {
                this.Reset(maxBufferNum, maxEnergyPerBuffer);
            }

            public void Reset(int maxBufferNum, float maxEnergyPerBuffer)
            {
                this.maxBufferNum = maxBufferNum;
                this.maxEnergyPerBuffer = maxEnergyPerBuffer;
            }

            public void AddEnergy(float energy)
            {
                if (this.currentBufferNum >= this.maxBufferNum)
                {
                    this.currentEnergy = 0f;
                    return;
                }
                this.currentEnergy += energy;
                if (this.currentEnergy >= this.maxEnergyPerBuffer)
                {
                    this.currentEnergy -= this.maxEnergyPerBuffer;
                    this.currentBufferNum++;
                    if (this.currentBufferNum >= this.maxBufferNum)
                    {
                        this.currentEnergy = 0f;
                    }
                }
            }

            public float GetEnergyAmount()
            {
                return this.currentEnergy / this.maxEnergyPerBuffer;
            }
        }

        protected Dictionary<TBufferType, EnergySlot<TBufferType>.SlotData> mBuffer = new Dictionary<TBufferType, EnergySlot<TBufferType>.SlotData>();

        public void AddBuffer(TBufferType bufferType, int maxBufferNum, float maxEnergyPerBuffer)
        {
            EnergySlot<TBufferType>.SlotData slotData = null;
            if (this.mBuffer.TryGetValue(bufferType, out slotData))
            {
                slotData.Reset(maxBufferNum, maxEnergyPerBuffer);
                return;
            }
            this.mBuffer.Add(bufferType, new EnergySlot<TBufferType>.SlotData(maxBufferNum, maxEnergyPerBuffer));
        }

        public void AddEnergy(TBufferType bufferType, float energy)
        {
            EnergySlot<TBufferType>.SlotData slotData = null;
            if (this.mBuffer.TryGetValue(bufferType, out slotData))
            {
                slotData.AddEnergy(energy);
            }
        }

        public int GetBufferNum(TBufferType bufferType)
        {
            EnergySlot<TBufferType>.SlotData slotData = null;
            if (this.mBuffer.TryGetValue(bufferType, out slotData))
            {
                return slotData.currentBufferNum;
            }
            return 0;
        }

        public float GetBufferEnergy(TBufferType bufferType)
        {
            EnergySlot<TBufferType>.SlotData slotData = null;
            if (this.mBuffer.TryGetValue(bufferType, out slotData))
            {
                return slotData.currentEnergy;
            }
            return 0f;
        }

        public void UseBuffer(TBufferType bufferType, int bufferNum)
        {
            EnergySlot<TBufferType>.SlotData slotData = null;
            if (this.mBuffer.TryGetValue(bufferType, out slotData))
            {
                slotData.currentBufferNum -= bufferNum;
                if (slotData.currentBufferNum < 0)
                {
                    slotData.currentBufferNum = 0;
                }
            }
        }

        public float GetEnergyAmount(TBufferType bufferType)
        {
            EnergySlot<TBufferType>.SlotData slotData = null;
            if (this.mBuffer.TryGetValue(bufferType, out slotData))
            {
                return slotData.GetEnergyAmount();
            }
            return 0f;
        }
    }
}
