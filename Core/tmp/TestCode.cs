using UnityEngine;
using System.Collections;
using GameCore.FSM;

public class TestCode : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TestStateMachine();
	}
	
	// Update is called once per frame
    public string debugStr;
	void Update () {
        actor.Tick(Time.deltaTime);
        debugStr = actor.state.CurrentState.ToString();
	}

    public Director actor;
    void TestStateMachine()
    {
        actor = new Director();
    }


}
