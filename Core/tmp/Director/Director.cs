using UnityEngine;
using System.Collections;
using GameCore.FSM;

public class Director {

    public Director()
    {
        state = new StateMachine<Director>(this);
        state.SetCurrentState(DirectorStateMachine1.Instance);

        entityManager = EntityManager<Actor>.CreateEntityManager();
        messageDispatcher = MessageDispatcher<Actor>.CreateMessageDispatcher(entityManager);
        helper = new DirectorHelper(this);
        helper.CreateActor();
    }

    public float fTime;
    public StateMachine<Director> state;
    public void Tick(float deltaTime)
    {
        state.Tick(deltaTime);
        foreach(Actor item in entityManager)
        {
            item.Tick(deltaTime);
        }
    }


    public EntityManager<Actor> entityManager;
    public MessageDispatcher<Actor> messageDispatcher;

    public DirectorHelper helper;
}
