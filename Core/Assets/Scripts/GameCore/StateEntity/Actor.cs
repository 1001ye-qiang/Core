using UnityEngine;
using System.Collections;
using GameCore.FSM;
using FuckGame.View;

public class Actor : GameEntity<ActorInfo, ActorConfig, Director>
{
    public StateMachine<Actor> stateMachine
    {
        get
        {
            if (mStateMachine == null) mStateMachine = new StateMachine<Actor>(this);
            return mStateMachine;
        }
    }
    private StateMachine<Actor> mStateMachine;

    public Actor(ActorInfo actorInfo, ActorConfig actorConfig, int actId, Director competitionDirector)
        : base(actorInfo, actorConfig, competitionDirector, actId)
    {

    }


    public AnimatorControl ani;
    public ViewMonitor vm;


    
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


