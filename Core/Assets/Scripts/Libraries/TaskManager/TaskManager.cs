using System;
using System.Collections.Generic;

namespace GameCore.Core
{
    public delegate void OnWork();
    public delegate bool OnWorkTick(float deltaTime);

    public class WorkManager
    {
        protected class Work
        {
            private OnWork OnEnable;

            private OnWork OnDisable;

            private OnWorkTick OnUpdate;

            public Work(OnWork onEnable, OnWorkTick onUpdate, OnWork onDisable)
            {
                this.OnEnable = onEnable;
                this.OnUpdate = onUpdate;
                this.OnDisable = onDisable;
            }

            public void Enable()
            {
                if (this.OnEnable != null)
                {
                    this.OnEnable();
                }
            }

            public void Disable()
            {
                if (this.OnDisable != null)
                {
                    this.OnDisable();
                }
            }

            public bool Tick(float deltaTime)
            {
                return this.OnUpdate == null || this.OnUpdate(deltaTime);
            }
        }

        private static WorkManager inst;

        private Queue<WorkManager.Work> queTasker = new Queue<WorkManager.Work>();
        // 这样写的好处是，不用每次NEW Queue
        private Queue<WorkManager.Work> queCache = new Queue<WorkManager.Work>();

        public static WorkManager Instance
        {
            get
            {
                if (WorkManager.inst == null)
                {
                    WorkManager.inst = new WorkManager();
                }
                return WorkManager.inst;
            }
        }

        public void Plugin(OnWorkTick onUpdate)
        {
            this.Plugin(null, null, onUpdate);
        }

        public void Plugin(OnWork onEnable, OnWork onDisable, OnWorkTick onUpdate)
        {
            WorkManager.Work work = new WorkManager.Work(onEnable, onUpdate, onDisable);
            work.Enable();
            this.queTasker.Enqueue(work);
        }

        public void Tick(float deltaTime)
        {
            while (this.queTasker.Count > 0)
            {
                WorkManager.Work work = this.queTasker.Dequeue();
                if (work.Tick(deltaTime))
                {
                    work.Disable();
                }
                else
                {
                    this.queCache.Enqueue(work);
                }
            }
            Queue<WorkManager.Work> queue = this.queTasker;
            this.queTasker = this.queCache;
            this.queCache = queue;
        }
    }
}
