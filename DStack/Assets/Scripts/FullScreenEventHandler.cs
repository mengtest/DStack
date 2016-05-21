using UnityEngine;
using System.Collections;

public class FullScreenEventHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPress(bool state){
		Debug.Log(" on press : "+state);
		if(state) PlaySceneCtr.Instance().MoveCubeFallDown();
	}
}
