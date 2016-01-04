using System;

namespace FuckGame.FSM
{
    public abstract class BaseGameEntity
    {
        private int mID;

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        protected void SetID(int val)
        {
            this.mID = val;
        }

        public virtual bool HandleMessage(Telegram msg)
        {
            return false;
        }
    }
}
