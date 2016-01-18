using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DirectorHelper
{
    public static int CreateActor(RoleData rd, OnFinish finish)
    {
        int id = PlayScript.Instance.Director.entityManager.CreateID();
        //Actor actor = new Actor()

        return id;
    }

    public static int[] LoadActors(List<RoleData> rds, OnFinish finish)
    {
        List<int> roleIds = new List<int>();
        for (int i = 0; i < rds.Count; ++i)
        {
            RoleData role = rds[i];
            ActorInfo actorInfo = new ActorInfo();
            /*
            actorInfo.hp = role.maxBlood; // set current max blood
            actorInfo.hp = role.blood;
            //actorInfo.stiffValue = role.stiffValue;
            actorInfo.campType = role.camp;
            actorInfo.localIndex = role.localIndex;
            actorInfo.monsterPackageIndex = role.monsterPackageIndex;
            actorInfo.unitType = role.unitType;
            actorInfo.des = role.des;
            actorInfo.teamId = role.teamId;
            actorInfo.battleInfo = role.battleInfo;
            actorInfo.beGoodStatus = role.beGoodStatus;
            actorInfo.handleInfos = role.handleInfos;
            actorInfo.skillInfos = role.skillInfos;
            actorInfo.lookDistance = role.lookDistance;
            //actorInfo.badStateInfos = role.badStsteInfo; // buffer

            actorInfo.skillWeight = role.skillWeight;
            actorInfo.targetPos = format[role.localIndex].pos;
            actorInfo.startAngle = format[role.localIndex].angle;

            ActorConfig actorConfig = new ActorConfig();


            actorConfig.assetName = role.asset;

            int actorID = CompetitionManager.Instance.world.CreateActor(actorInfo, actorConfig, format[role.localIndex].pos, Quaternion.Euler(0, format[role.localIndex].angle, 0), role.ReadConfig, delegate(bool isLoad)
            {
                if (--remain <= 0)
                    onFinish(true);
            });
            roleIds.Add(actorID);
            */
        }
        return roleIds.ToArray();
    }
}
