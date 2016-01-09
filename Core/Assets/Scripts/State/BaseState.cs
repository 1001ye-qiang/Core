using UnityEngine;
using System.Collections;

using GameCore.FSM;

public abstract class BaseState<T, U> : State<U> where T : new()
{
    private static T inst;
    public static T Instance
    {
        get{
            if (inst == null)
                inst = new T();
            return inst;
        }
    }

    public abstract void Enter(U entity);
    public abstract void Execute(U entity, float deltaTime);
    public abstract void Exit(U entity);
    public abstract bool OnMessage(U entity, Telegram msg);
}