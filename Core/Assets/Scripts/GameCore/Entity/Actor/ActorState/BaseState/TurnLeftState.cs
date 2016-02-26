using UnityEngine;
using System.Collections;
using GameCore.FSM;

public class TurnLeftState : BaseState<TurnLeftState, Actor>
{

    //进入状态时调用的方法//
    override public void Enter(Actor entity)
    {
    }

    //在当前状态的更新方法中一直调用//
    override public void Execute(Actor entity, float deltaTime)
    {
    }

    //退出状态时调用的方法//
    override public void Exit(Actor entity)
    {
    }

    //如果智能体从消息发送器中接收了一条消息，会执行此方法//
    override public bool OnMessage(Actor entity, Telegram msg)
    {
        switch ((ActorMsgType)msg.mMsg)
        {
            case ActorMsgType.Idle:
                entity.stateMachine.ChangeState(IdleState.Instance);
                return true;
            case ActorMsgType.Run:
                entity.stateMachine.ChangeState(MoveState.Instance);
                return true;
        }
        return false;
    }
}
