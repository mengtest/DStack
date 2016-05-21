using UnityEngine;
using System.Collections;

public class GameResultCtr : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnRestart(){

		this.gameObject.SetActive(false);
		PlaySceneCtr.Instance().OnRestart();
	}
}
