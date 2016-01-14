using UnityEngine;
using System.Collections;


namespace FuckGame.View
{
    public delegate void OnEvent(string param);
    //public delegate void OnDestroy(IViewEntity viewEntity);
    public delegate void OnCreateFinish(bool isFinish, int viewId);
    public delegate bool OnColliderEvent(GameObject trigger, GameObject collider);
    public delegate Vector3 OnAniMove(Vector3 pos);
    public delegate void OnAniEvent();
}