using System;

namespace FuckGame.FSM
{
    public interface State<TGameEntity>
    {
        void Enter(TGameEntity entity);

        void Execute(TGameEntity entity, float deltaTime);

        void Exit(TGameEntity entity);

        bool OnMessage(TGameEntity entity, Telegram msg);
    }
}
