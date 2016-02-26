using UnityEngine;
using System.Collections;

public class DirectorHelper {
    public Director director;
    public DirectorHelper(Director d)
    {
        director = d;
    }
    public void CreateActor()
    {
        int actorID = director.entityManager.CreateID();
        Actor actor = new Actor(new ActorInfo(), new ActorConfig(), director, actorID);
    }
}
