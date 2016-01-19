using UnityEngine;
using System.Collections;

public class GameEnter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        PlayScript.Instance.director.Tick(Time.deltaTime);
	}
}
