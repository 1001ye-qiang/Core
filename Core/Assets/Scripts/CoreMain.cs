using UnityEngine;
using System.Collections;
using FuckGame.Res;

public class CoreMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        FuckGame.Res.PackageManager.Instance.Initialize(AddressConfig.ResourceLocalURL, AddressConfig.ResourceLocalPath, AddressConfig.ResourceBaseURL);
	}
	
	// Update is called once per frame
	void Update () {
        FuckGame.Res.PackageManager.Instance.Tick(Time.deltaTime);
        GameCore.Core.WorkManager.Instance.Tick(Time.deltaTime);
        PlayScript.Instance.director.Tick(Time.deltaTime);
	}
}
