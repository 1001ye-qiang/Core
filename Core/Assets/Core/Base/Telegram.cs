using System;
using UnityEngine;

namespace FuckGame.FSM
{
    public class Telegram : IComparable
    {
        private const float mSmallestDelay = 0.25f;

        public int mSender;

        public int mReceiver;

        public int mMsg;

        public float mDispatchTime;

        public object[] mExtraInfo;

        public Telegram(float delay, int sender, int receiver, int msg, object[] extraInfo)
        {
            this.mDispatchTime = delay;
            this.mSender = sender;
            this.mReceiver = receiver;
            this.mMsg = msg;
            this.mExtraInfo = extraInfo;
        }

        public int CompareTo(object obj)
        {
            Telegram telegram = (Telegram)obj;
            if (telegram == this)
            {
                Debug.LogWarning("comparing to same obj");
            }
            return (int)(this.mDispatchTime - telegram.mDispatchTime);
        }

        public override bool Equals(object obj)
        {
            Telegram telegram = obj as Telegram;
            return telegram != null && telegram.mDispatchTime > 0f && this.mDispatchTime > 0f && (telegram.mSender == this.mSender && telegram.mReceiver == this.mReceiver && telegram.mMsg == this.mMsg && Mathf.Abs(telegram.mDispatchTime - this.mDispatchTime) < 0.25f);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
