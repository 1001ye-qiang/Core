using UnityEngine;
using System.Collections;
using GameCore.FSM;
using FuckGame.View;

public class Actor : GameEntity<ActorInfo, ActorConfig, Director>
{
    #region base
    public StateMachine<Actor> stateMachine
    {
        get
        {
            if (mStateMachine == null) mStateMachine = new StateMachine<Actor>(this);
            return mStateMachine;
        }
    }
    private StateMachine<Actor> mStateMachine;
    #endregion // base

    #region public editor member
    public AnimatorControl ani;
    public ViewMonitor vm;
    #endregion // public editor member

    public Actor(ActorInfo actorInfo, ActorConfig actorConfig, int actId, Director competitionDirector)
        : base(actorInfo, actorConfig, competitionDirector, actId)
    {

    }




    
}

public class AnimatorControl
{
}
public class ColliderControl
{
}

public class ActorInfo
{ }
public class ActorConfig
{ }

public enum ActorMsgType
{
    Idle,
    Run,
    TurnLeft,
    TurnRight,
    TurnAround
}


