using System;

namespace GameCore.FSM
{
    public class StateMachine<TGameEntity>
    {
        private TGameEntity mOwner;

        private State<TGameEntity> mCurrentState;

        private State<TGameEntity> mPreviousState;

        private State<TGameEntity> mGlobalState;

        public State<TGameEntity> CurrentState
        {
            get
            {
                return this.mCurrentState;
            }
        }

        public State<TGameEntity> PreviousState
        {
            get
            {
                return this.mPreviousState;
            }
        }

        public State<TGameEntity> GlobalState
        {
            get
            {
                return this.mGlobalState;
            }
        }

        public StateMachine(TGameEntity owner)
        {
            this.mOwner = owner;
            this.mCurrentState = null;
            this.mPreviousState = null;
            this.mGlobalState = null;
        }

        public void SetCurrentState(State<TGameEntity> s)
        {
            this.mCurrentState = s;
            this.mCurrentState.Enter(this.mOwner);
        }

        public void SetPreviousState(State<TGameEntity> s)
        {
            this.mPreviousState = s;
        }

        public void SetGlobalState(State<TGameEntity> s)
        {
            this.mGlobalState = s;
            this.mGlobalState.Enter(this.mOwner);
        }

        public void Tick(float deltaTime)
        {
            if (this.mGlobalState != null)
            {
                this.mGlobalState.Execute(this.mOwner, deltaTime);
            }
            if (this.mCurrentState != null)
            {
                this.mCurrentState.Execute(this.mOwner, deltaTime);
            }
        }

        public void ChangeState(State<TGameEntity> newState)
        {
            this.mPreviousState = this.mCurrentState;
            this.mCurrentState.Exit(this.mOwner);
            this.mCurrentState = newState;
            this.mCurrentState.Enter(this.mOwner);
        }

        public void RevertToPreviousState()
        {
            this.ChangeState(this.mPreviousState);
        }

        public bool IsInState(State<TGameEntity> st)
        {
            return st == this.mCurrentState;
        }

        public bool HandleMessage(Telegram msg)
        {
            return (this.mCurrentState != null && this.mCurrentState.OnMessage(this.mOwner, msg)) || (this.mGlobalState != null && this.mGlobalState.OnMessage(this.mOwner, msg));
        }
    }
}
