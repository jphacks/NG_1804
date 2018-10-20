using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

public class GameManager : MonoBehaviour {
	public AREventManager AREM;

	enum GameState{
		Idle, WorldMapInit, Playering
	}
	string[] madeObjectIDs;
	public Dictionary<string, string> dicIDs = new Dictionary<string, string>();
	public bool hasStarted = false;
	GameState state = GameState.Idle;

	// Use this for initialization
	void Start () {

	}

	public void AddDicIDs(string anchorid, string objectid){
		dicIDs.Add (anchorid, objectid);
	}

	// Update is called once per frame
	void Update () {
		switch (state) {
		case GameState.Idle:
			StartCoroutine (DataLoadCoroutine ());
			state = GameState.WorldMapInit;
			break;
		case GameState.WorldMapInit:
			
			break;
		case GameState.Playering:
			break;
		}
	}

	IEnumerator DataLoadCoroutine(){
		
		if (!hasStarted) {
			hasStarted = true;
		}
		while (true) {

			yield return new WaitForSeconds (10);
		}

	}
}
