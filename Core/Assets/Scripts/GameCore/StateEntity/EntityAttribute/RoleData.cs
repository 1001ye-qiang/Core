using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class RoleData{
    public string strResourceName;              // 模型名字
    public int iHp;                             
    public int iMaxHp;                          // 血量
    public List<SkillData> lstSkillData;        // 技能数值
    public float fMoveSpeed;                    // 移动速度
    public Vector3 v3StartPos;
    public Vector3 v3StartRotation; // 位置信息
    public int iLoadEvent; // 触发事件ID
    public int iLoadSeconds; // 触发后多长时间生成
    public int iRatrols; // 巡逻范围

    // 阵营
    public RoleType roleType = RoleType.Monster_stand;
    public RoleCampType roleCampType = RoleCampType.Black;
    public int iTeamId;

    // 
}

[System.Serializable]
public class SkillData
{
    public int iBaseDamage;
    public float fCriticalRate;
    public float fCoolTime;
}

[System.Serializable]
public enum RoleType
{
    Monster_stand,
    Monster_boss,
    Monster_special,

    Hero,
    Pet,
    Summon,
}

[System.Serializable]
public enum RoleCampType
{
    Red,
    Black,
    Other
}
