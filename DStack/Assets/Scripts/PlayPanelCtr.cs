using UnityEngine;
using System.Collections;

public class PlayPanelCtr : MonoBehaviour {

	public UILabel score_UILabel;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateScore(int score){
		score_UILabel.text="Score: "+score;
	}
}
