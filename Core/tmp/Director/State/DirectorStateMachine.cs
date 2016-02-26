using UnityEngine;
using System.Collections;
using GameCore.FSM;

public class DirectorStateMachine1 : BaseState<DirectorStateMachine1, Director>
{
    //进入状态时调用的方法//
    override public void Enter(Director entity)
    {
    }

    //在当前状态的更新方法中一直调用//
    override public void Execute(Director entity, float deltaTime)
    {
        entity.fTime += deltaTime;
        if(entity.fTime > 2f)
        {
            entity.fTime = 0f;
            entity.state.ChangeState(DirectorStateMachine2.Instance);
        }
    }

    //退出状态时调用的方法//
    override public void Exit(Director entity)
    {
    }

    //如果智能体从消息发送器中接收了一条消息，会执行此方法//
    override public bool OnMessage(Director entity, Telegram msg)
    {
        return base.OnMessage(entity, msg);
    }

}
public class DirectorStateMachine2 : BaseState<DirectorStateMachine2, Director>
{
    //进入状态时调用的方法//
    override public void Enter(Director entity)
    {
    }

    //在当前状态的更新方法中一直调用//
    override public void Execute(Director entity, float deltaTime)
    {
        entity.fTime += deltaTime;
        if (entity.fTime > 2f)
        {
            entity.fTime = 0f;
            entity.state.ChangeState(DirectorStateMachine3.Instance);
        }
    }

    //退出状态时调用的方法//
    override public void Exit(Director entity)
    {
    }

    //如果智能体从消息发送器中接收了一条消息，会执行此方法//
    override public bool OnMessage(Director entity, Telegram msg)
    {
        return base.OnMessage(entity, msg);
    }

}
public class DirectorStateMachine3 : BaseState<DirectorStateMachine3, Director>
{

    //进入状态时调用的方法//
    override public void Enter(Director entity)
    {
    }

    //在当前状态的更新方法中一直调用//
    override public void Execute(Director entity, float deltaTime)
    {
        entity.fTime += deltaTime;
        if (entity.fTime > 2f)
        {
            entity.fTime = 0f;
            entity.state.ChangeState(DirectorStateMachine4.Instance);
        }
    }

    //退出状态时调用的方法//
    override public void Exit(Director entity)
    {
    }

    //如果智能体从消息发送器中接收了一条消息，会执行此方法//
    override public bool OnMessage(Director entity, Telegram msg)
    {
        return base.OnMessage(entity, msg);
    }
}
public class DirectorStateMachine4 : BaseState<DirectorStateMachine4, Director>
{
    //进入状态时调用的方法//
    override public void Enter(Director entity)
    {
    }

    //在当前状态的更新方法中一直调用//
    override public void Execute(Director entity, float deltaTime)
    {
        entity.fTime += deltaTime;
        if (entity.fTime > 2f)
        {
            entity.fTime = 0f;
            entity.state.ChangeState(DirectorStateMachine5.Instance);
        }
    }

    //退出状态时调用的方法//
    override public void Exit(Director entity)
    {
    }

    //如果智能体从消息发送器中接收了一条消息，会执行此方法//
    override public bool OnMessage(Director entity, Telegram msg)
    {
        return base.OnMessage(entity, msg);
    }

}
public class DirectorStateMachine5 : BaseState<DirectorStateMachine5, Director>
{
    //进入状态时调用的方法//
    override public void Enter(Director entity)
    {
    }

    //在当前状态的更新方法中一直调用//
    override public void Execute(Director entity, float deltaTime)
    {
        entity.fTime += deltaTime;
        if (entity.fTime > 2f)
        {
            entity.fTime = 0f;
            entity.state.ChangeState(DirectorStateMachine1.Instance);
        }
    }

    //退出状态时调用的方法//
    override public void Exit(Director entity)
    {
    }

    //如果智能体从消息发送器中接收了一条消息，会执行此方法//
    override public bool OnMessage(Director entity, Telegram msg)
    {
        return base.OnMessage(entity, msg);
    }

}
