using UnityEngine;
using System.Collections;
using GameCore.FSM;
using FuckGame.View;
using System.Collections.Generic;

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

[System.Serializable]
public class ActorInfo
{
    public string strResourceName;              // 模型名字
    public int iHp;
    public int iMaxHp;                          // 血量
    public List<SkillData> lstSkillData;        // 技能数值
    public float fMoveSpeed;                    // 移动速度
    public Vector3 v3StartPos;
    public Vector3 v3StartRotation; // 位置信息
    public int iLoadEvent; // 触发事件ID
    public int iLoadSeconds; // 触发后多长时间生成

    // 阵营
    public RoleType roleType = RoleType.Monster_stand;
    public RoleCampType roleCampType = RoleCampType.Black;
    public int iTeamId;
}
[System.Serializable]
public class ActorConfig
{
    public int iRatrols; // 巡逻范围
}

public enum ActorMsgType
{
    Idle,
    Run,
    TurnLeft,
    TurnRight,
    TurnAround
}


