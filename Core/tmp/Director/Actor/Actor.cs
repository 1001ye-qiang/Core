using UnityEngine;
using System.Collections;
using GameCore.FSM;

[SerializeField]
public class ActorConfig
{ }

[SerializeField]
public class ActorInfo
{ }

[SerializeField]
public class Actor : GameEntity<ActorInfo, ActorConfig, Director>
{
    public Actor(ActorInfo info, ActorConfig config, Director director, int id)
        : base(info, config, director, id)
    {
        state = new StateMachine<Actor>(this);
        state.SetCurrentState(ActorStateMachine1.Instance);


        director.entityManager.RegisterEntity(this);
    }

    public float fTime;
    public StateMachine<Actor> state;
    public void Tick(float deltaTime)
    {
        state.Tick(deltaTime);
    }
    public override bool HandleMessage(Telegram msg)
    {
        return state.HandleMessage(msg);
    }
}
