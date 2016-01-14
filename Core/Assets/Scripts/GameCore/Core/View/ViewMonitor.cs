using System;
using System.Collections.Generic;
using UnityEngine;

namespace FuckGame.View
{
    public class ViewMonitor : MonoBehaviour
    {
        private Animator animator;

        private Dictionary<string, OnEvent[]> viewEvent = new Dictionary<string, OnEvent[]>();

        private Dictionary<int, OnAniEvent[]> aniEvent = new Dictionary<int, OnAniEvent[]>();

        private OnColliderEvent[] colliderEvent;

        private OnAniMove onAniMove;

        protected AnimatorStateInfo preFrameAnimatorState;

        private Dictionary<Type, object> currentTriggerEnable = new Dictionary<Type, object>();

        private Dictionary<Type, object> currentTriggerDisable = new Dictionary<Type, object>();

        private Dictionary<Type, object> currentTriggerUpdate = new Dictionary<Type, object>();

        private object currentColliderEnter;

        private object currentColliderExit;

        public virtual void Initialize()
        {
            this.animator = base.GetComponentInChildren<Animator>();
        }

        public void NotifyEventStart(string message)
        {
            OnEvent[] array = null;
            string[] array2 = message.Split(new char[]
			{
				':'
			});
            if (this.viewEvent.TryGetValue(array2[0], out array) && array[0] != null)
            {
                array[0]((array2.Length == 2) ? array2[1] : null);
            }
        }

        public void NotifyEventEnd(string message)
        {
            OnEvent[] array = null;
            string[] array2 = message.Split(new char[]
			{
				':'
			});
            if (this.viewEvent.TryGetValue(array2[0], out array) && array[1] != null)
            {
                array[1]((array2.Length == 2) ? array2[1] : null);
            }
        }

        public void PluginEventMonitor(string viewEventName, OnEvent onEventStart, OnEvent onEventEnd)
        {
            OnEvent[] value = new OnEvent[]
			{
				onEventStart,
				onEventEnd
			};
            this.viewEvent.Add(viewEventName, value);
        }

        public void CancleEventMonitor(string viewEventName)
        {
            this.viewEvent.Remove(viewEventName);
        }

        public void PluginAniMonitor(string aniName, OnAniEvent onStart, OnAniEvent onFrame, OnAniEvent onEnd)
        {
            AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
            int animatorHash = this.GetAnimatorHash(aniName);
            this.aniEvent.Add(animatorHash, new OnAniEvent[]
			{
				onStart,
				onFrame,
				onEnd
			});
            if (currentAnimatorStateInfo.fullPathHash == animatorHash)
            {
                if (this.preFrameAnimatorState.fullPathHash != animatorHash)
                {
                    OnAniEvent[] array = null;
                    this.aniEvent.TryGetValue(this.preFrameAnimatorState.fullPathHash, out array);
                    if (array != null && array[2] != null)
                    {
                        array[2]();
                    }
                }
                if (onStart != null)
                {
                    onStart();
                }
            }
        }

        public void CancleAniMonitor(string aniName)
        {
            this.aniEvent.Remove(this.GetAnimatorHash(aniName));
        }

        public void PluginTriggerMonitor<T>(ViewTrigger<T>.OnTrigger onEnable, ViewTrigger<T>.OnTrigger onUpdate, ViewTrigger<T>.OnTrigger onDisable)
        {
            Type typeFromHandle = typeof(T);
            this.currentTriggerEnable.Add(typeFromHandle, onEnable);
            this.currentTriggerUpdate.Add(typeFromHandle, onUpdate);
            this.currentTriggerDisable.Add(typeFromHandle, onDisable);
        }

        public void CancleTriggerMonitor<T>()
        {
            Type typeFromHandle = typeof(T);
            this.currentTriggerEnable.Remove(typeFromHandle);
            this.currentTriggerDisable.Remove(typeFromHandle);
            this.currentTriggerUpdate.Remove(typeFromHandle);
        }

        public ViewTrigger<T>.OnTrigger GetTriggerEnable<T>()
        {
            Type typeFromHandle = typeof(T);
            object obj = null;
            this.currentTriggerEnable.TryGetValue(typeFromHandle, out obj);
            return (ViewTrigger<T>.OnTrigger)obj;
        }

        public ViewTrigger<T>.OnTrigger GetTriggerDisable<T>()
        {
            Type typeFromHandle = typeof(T);
            object obj = null;
            this.currentTriggerDisable.TryGetValue(typeFromHandle, out obj);
            return (ViewTrigger<T>.OnTrigger)obj;
        }

        public ViewTrigger<T>.OnTrigger GetTriggerUpdate<T>()
        {
            Type typeFromHandle = typeof(T);
            object obj = null;
            this.currentTriggerUpdate.TryGetValue(typeFromHandle, out obj);
            return (ViewTrigger<T>.OnTrigger)obj;
        }

        public void PluginColliderMonitor<T>(ViewTrigger<T>.OnColidder onColliderEnter, ViewTrigger<T>.OnColidder onColliderExit) where T : MonoBehaviour
        {
            this.currentColliderEnter = onColliderEnter;
            this.currentColliderExit = onColliderExit;
        }

        public void CancleColliderMonitor<T>()
        {
            this.currentColliderEnter = null;
            this.currentColliderExit = null;
        }

        public ViewTrigger<T>.OnColidder GetColliderEnter<T>()
        {
            if (this.currentColliderEnter != null)
            {
                return (ViewTrigger<T>.OnColidder)this.currentColliderEnter;
            }
            return null;
        }

        public ViewTrigger<T>.OnColidder GetColliderExit<T>()
        {
            if (this.currentColliderExit != null)
            {
                return (ViewTrigger<T>.OnColidder)this.currentColliderExit;
            }
            return null;
        }

        public virtual void PluginAniMoveMonitor(OnAniMove onAniMove)
        {
            this.onAniMove = onAniMove;
        }

        public void CancleAniMoveMonitor()
        {
            this.onAniMove = null;
        }

        public Vector3 ViewAniMove(Vector3 pos)
        {
            if (this.onAniMove != null)
            {
                return this.onAniMove(pos);
            }
            return Vector3.zero;
        }

        public void RefreshEvent()
        {
            if (this.animator == null || this.animator.IsInTransition(0))
            {
                return;
            }
            AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
            OnAniEvent[] array = null;
            this.aniEvent.TryGetValue(currentAnimatorStateInfo.fullPathHash, out array);
            if (this.preFrameAnimatorState.fullPathHash != currentAnimatorStateInfo.fullPathHash)
            {
                OnAniEvent[] array2 = null;
                this.aniEvent.TryGetValue(this.preFrameAnimatorState.fullPathHash, out array2);
                if (array2 != null && array2[2] != null)
                {
                    array2[2]();
                }
                if (array != null && array[0] != null)
                {
                    array[0]();
                }
            }
            else if (array != null && array[1] != null)
            {
                array[1]();
            }
            this.preFrameAnimatorState = currentAnimatorStateInfo;
        }

        protected int GetAnimatorHash(string animeName)
        {
            return this.GetAnimatorHash("Base Layer", animeName);
        }

        protected int GetAnimatorHash(string layerName, string animeName)
        {
            return Animator.StringToHash(layerName + "." + animeName);
        }
    }
}
