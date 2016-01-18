using UnityEngine;
using System.Collections;
using GameCore.FSM;

public class RunState : BaseState<RunState, Director>
{
    //进入状态时调用的方法//
    override public void Enter(Director entity)
    {
    }

    //在当前状态的更新方法中一直调用//
    override public void Execute(Director entity, float deltaTime)
    {
        foreach (Actor actor in entity.entityManager)
        {
            actor.stateMachine.Tick(deltaTime);
        }
    }

    //退出状态时调用的方法//
    override public void Exit(Director entity)
    {
    }

    //如果智能体从消息发送器中接收了一条消息，会执行此方法//
    override public bool OnMessage(Director entity, Telegram msg)
    {
        switch ((DirectorMsgType)msg.mMsg)
        {
            case DirectorMsgType.Prepare:
                entity.stateMachine.ChangeState(PrepareState.Instance);
                return true;
            case DirectorMsgType.Run:
                entity.stateMachine.ChangeState(RunState.Instance);
                return true;
            case DirectorMsgType.Pause:
                entity.stateMachine.ChangeState(PauseState.Instance);
                return true;
            case DirectorMsgType.Restart:
                entity.stateMachine.ChangeState(RestartState.Instance);
                return true;
            case DirectorMsgType.Close:
                entity.stateMachine.ChangeState(CloseState.Instance);
                return true;
            case DirectorMsgType.SaveProcess:
                entity.stateMachine.ChangeState(SaveProcessState.Instance);
                return true;
        }
        return false;
    }
}