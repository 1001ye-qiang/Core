using UnityEngine;
using System.Collections;
using GameCore.FSM;

public class ActorStateMachine1 : BaseState<ActorStateMachine1, Actor>
{
    //进入状态时调用的方法//
    override public void Enter(Actor entity)
    {
    }

    //在当前状态的更新方法中一直调用//
    override public void Execute(Actor entity, float deltaTime)
    {
        entity.fTime += deltaTime;
        if(entity.fTime > 2f)
        {
            entity.fTime = 0f;
            entity.state.ChangeState(ActorStateMachine2.Instance);

            entity.director.messageDispatcher.DispatchMessage(0, -1, entity.ID, 1, null);
        }
    }

    //退出状态时调用的方法//
    override public void Exit(Actor entity)
    {
    }

    //如果智能体从消息发送器中接收了一条消息，会执行此方法//
    override public bool OnMessage(Actor entity, Telegram msg)
    {
        return base.OnMessage(entity, msg);
    }

}
public class ActorStateMachine2 : BaseState<ActorStateMachine2, Actor>
{
    //进入状态时调用的方法//
    override public void Enter(Actor entity)
    {
    }

    //在当前状态的更新方法中一直调用//
    override public void Execute(Actor entity, float deltaTime)
    {
        entity.fTime += deltaTime;
        if (entity.fTime > 2f)
        {
            entity.fTime = 0f;
            entity.state.ChangeState(ActorStateMachine3.Instance);

            entity.director.messageDispatcher.DispatchMessage(0, -1, entity.ID, 2, null);
        }
    }

    //退出状态时调用的方法//
    override public void Exit(Actor entity)
    {
    }

    //如果智能体从消息发送器中接收了一条消息，会执行此方法//
    override public bool OnMessage(Actor entity, Telegram msg)
    {
        return base.OnMessage(entity, msg);
    }

}
public class ActorStateMachine3 : BaseState<ActorStateMachine3, Actor>
{

    //进入状态时调用的方法//
    override public void Enter(Actor entity)
    {
    }

    //在当前状态的更新方法中一直调用//
    override public void Execute(Actor entity, float deltaTime)
    {
        entity.fTime += deltaTime;
        if (entity.fTime > 2f)
        {
            entity.fTime = 0f;
            entity.state.ChangeState(ActorStateMachine4.Instance);

            entity.director.messageDispatcher.DispatchMessage(0, -1, entity.ID, 3, null);
        }
    }

    //退出状态时调用的方法//
    override public void Exit(Actor entity)
    {
    }

    //如果智能体从消息发送器中接收了一条消息，会执行此方法//
    override public bool OnMessage(Actor entity, Telegram msg)
    {
        return base.OnMessage(entity, msg);
    }
}
public class ActorStateMachine4 : BaseState<ActorStateMachine4, Actor>
{
    //进入状态时调用的方法//
    override public void Enter(Actor entity)
    {
    }

    //在当前状态的更新方法中一直调用//
    override public void Execute(Actor entity, float deltaTime)
    {
        entity.fTime += deltaTime;
        if (entity.fTime > 2f)
        {
            entity.fTime = 0f;
            entity.state.ChangeState(ActorStateMachine5.Instance);

            entity.director.messageDispatcher.DispatchMessage(0, -1, entity.ID, 4, null);
        }
    }

    //退出状态时调用的方法//
    override public void Exit(Actor entity)
    {
    }

    //如果智能体从消息发送器中接收了一条消息，会执行此方法//
    override public bool OnMessage(Actor entity, Telegram msg)
    {
        return base.OnMessage(entity, msg);
    }

}
public class ActorStateMachine5 : BaseState<ActorStateMachine5, Actor>
{
    //进入状态时调用的方法//
    override public void Enter(Actor entity)
    {
    }

    //在当前状态的更新方法中一直调用//
    override public void Execute(Actor entity, float deltaTime)
    {
        entity.fTime += deltaTime;
        if (entity.fTime > 2f)
        {
            entity.fTime = 0f;
            entity.state.ChangeState(ActorStateMachine1.Instance);

            entity.director.messageDispatcher.DispatchMessage(0, -1, entity.ID, 5, null);
        }
    }

    //退出状态时调用的方法//
    override public void Exit(Actor entity)
    {
    }

    //如果智能体从消息发送器中接收了一条消息，会执行此方法//
    override public bool OnMessage(Actor entity, Telegram msg)
    {
        return base.OnMessage(entity, msg);
    }

}
