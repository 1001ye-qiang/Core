using UnityEngine;
using System.Collections;
using GameCore.FSM;

public class Director {

    public StateMachine<Director> stateMachine
    {
        get
        {
            if (mStateMachine == null) mStateMachine = new StateMachine<Director>(this);
            return mStateMachine;
        }
    }
    private StateMachine<Director> mStateMachine;

    //管理比赛角色
    public EntityManager<Actor> entityManager
    {
        get
        {
            if (mEntityManager == null) mEntityManager = EntityManager<Actor>.CreateEntityManager();
            return mEntityManager;
        }
    }
    private EntityManager<Actor> mEntityManager;

    public MessageDispatcher<Actor> entityDispatcher
    {
        get
        {
            if (mEntityMessageDispatcher == null) mEntityMessageDispatcher = MessageDispatcher<Actor>.CreateMessageDispatcher(entityManager);
            return mEntityMessageDispatcher;
        }
    }
    //角色之间的通信
    private MessageDispatcher<Actor> mEntityMessageDispatcher;

}
//导演响应的消息
public enum DirectorMsgType
{
    Run = 0,
    Pause = 1,
}


