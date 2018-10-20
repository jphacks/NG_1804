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
	Dictionary<string, string> dicIDs;
	public bool hasStarted = false;
	GameState state = GameState.Idle;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		switch (state) {
		case GameState.Idle:
			state = GameState.Playering;
			break;
		case GameState.WorldMapInit:
			StartCoroutine(DataLoadCoroutine ());
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
