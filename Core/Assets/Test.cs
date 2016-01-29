using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {


    void OnEnable()
    {
        Transform t = Common.Root;
        t = Common.ActorArea;
        t = Common.FactoryArea;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
