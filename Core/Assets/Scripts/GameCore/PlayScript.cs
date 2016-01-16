using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayScript : SingleBase<PlayScript> {

    private Director director;
    public Director Director
    {
        get { return director; }
    }

    public List<RoleData> lstRD;
    public StageInfo stageRes;

    // public                           // 这一集的基本节奏？


    // 基本因素定义在剧本里，实现在导演那里。
    public void OpenScript(List<RoleData> lstRd, StageInfo si)
    {
        this.lstRD = lstRd;
        this.stageRes = si;
        if (director == null)
            director = new Director();

        GameObject stage = LoadPrefab.LoadResource(si.path); 
        StageConfig sc = stage.GetComponent<StageConfig>();


        director.InitDirector(lstRd, sc);
    }

    public void CloseScript()
    {
    }

    public void PauseScript()
    {
    }

    public void RestartScript()
    {
    }

}

