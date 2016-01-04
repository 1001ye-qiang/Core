using System;
using System.Collections;
using UnityEngine;

namespace FuckGame.FSM
{
    public class MessageDispatcher<TEntity> where TEntity : BaseGameEntity
    {
        public static readonly int SEND_MSG_IMMADEIATELY;

        private PriorityQueue<Telegram> mPriorityQ;

        private EntityManager<TEntity> mEntityManager;

        private float passTime;

        private void Discharge(TEntity receiverEntity, Telegram msg)
        {
            if (!receiverEntity.HandleMessage(msg))
            {
                Debug.Log("msg " + msg.mMsg + " not handled !");
            }
        }

        private void Discharge(int receiver, Telegram telegram)
        {
            TEntity entityFromID = this.mEntityManager.GetEntityFromID(receiver);
            TEntity arg_13_0 = entityFromID;
            this.Discharge(entityFromID, telegram);
        }

        private MessageDispatcher(EntityManager<TEntity> entityManager)
        {
            this.mPriorityQ = new PriorityQueue<Telegram>();
            this.mEntityManager = entityManager;
        }

        public static MessageDispatcher<TEntity> CreateMessageDispatcher(EntityManager<TEntity> entityManager)
        {
            return new MessageDispatcher<TEntity>(entityManager);
        }

        public void DispatchMessage(float delay, int sender, int receiver, int msg, object[] extraInfo)
        {
            Telegram telegram = new Telegram(0f, sender, receiver, msg, extraInfo);
            if (delay <= 0f)
            {
                if (receiver == -1)
                {
                    IEnumerator enumerator = this.mEntityManager.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            TEntity receiverEntity = (TEntity)((object)enumerator.Current);
                            this.Discharge(receiverEntity, telegram);
                        }
                        return;
                    }
                    finally
                    {
                        IDisposable disposable = enumerator as IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }
                }
                this.Discharge(receiver, telegram);
                return;
            }
            Debug.Log(string.Concat(new object[]
			{
				"Delayed telegram from ",
				sender,
				" recorded at time ",
				Time.get_realtimeSinceStartup(),
				" for ",
				receiver,
				". Msg is ",
				msg
			}));
            float num = this.passTime;
            telegram.mDispatchTime = num + delay;
            this.mPriorityQ.Enqueue(telegram);
        }

        public void Tick(float deltaTime)
        {
            this.passTime += deltaTime;
        }

        public void DispatchDelayedMessages()
        {
            float num = this.passTime;
            while (!this.mPriorityQ.Empty() && this.mPriorityQ.Peek().mDispatchTime < num && this.mPriorityQ.Peek().mDispatchTime > 0f)
            {
                Telegram telegram = this.mPriorityQ.Peek();
                if (telegram.mReceiver == -1)
                {
                    IEnumerator enumerator = this.mEntityManager.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            TEntity receiverEntity = (TEntity)((object)enumerator.Current);
                            this.Discharge(receiverEntity, telegram);
                        }
                        goto IL_6B;
                    }
                    finally
                    {
                        IDisposable disposable = enumerator as IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }
                    goto IL_5E;
                }
                goto IL_5E;
            IL_6B:
                this.mPriorityQ.Dequeue();
                continue;
            IL_5E:
                this.Discharge(telegram.mReceiver, telegram);
                goto IL_6B;
            }
        }
    }
}
