using UnityEngine;
using System.Collections;
using GameCore.FSM;
using System.Collections.Generic;

public class Director
{
    #region base
    // state machine
    public StateMachine<Director> stateMachine
    {
        get
        {
            if (mStateMachine == null) mStateMachine = new StateMachine<Director>(this);
            return mStateMachine;
        }
    }
    private StateMachine<Director> mStateMachine;

    // entity manager
    public EntityManager<Actor> entityManager
    {
        get
        {
            if (mEntityManager == null) mEntityManager = EntityManager<Actor>.CreateEntityManager();
            return mEntityManager;
        }
    }
    private EntityManager<Actor> mEntityManager;

    // role communication
    public MessageDispatcher<Actor> entityDispatcher
    {
        get
        {
            if (mEntityMessageDispatcher == null) mEntityMessageDispatcher = MessageDispatcher<Actor>.CreateMessageDispatcher(entityManager);
            return mEntityMessageDispatcher;
        }
    }
    private MessageDispatcher<Actor> mEntityMessageDispatcher;
    #endregion // base

    #region Config
    public List<RoleData> lstRD;
    public StageConfig stageConfig;

    public DirectorHelper helper { get { if (mHelper == null) mHelper = new DirectorHelper(this); return mHelper; } }
    DirectorHelper mHelper = null;
    #endregion // config

    
    // config machine
    // info machine
    // script
    public void OnAction()
    {

    }

    public void Tick(float deltaTime)
    {
        stateMachine.Tick(deltaTime);
        //entityDispatcher.Tick(deltaTime);
    }

}
//导演响应的消息
public enum DirectorMsgType
{
    Prepare = 0,
    Run,
    Pause,
    Restart,
    Close,
    SaveProcess,
}


