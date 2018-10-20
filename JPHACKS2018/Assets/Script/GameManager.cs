using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using UnityEngine.XR.iOS;
using UnityEngine.UI;
using System;

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
		/*
			//QueryTestを検索するクラスを作成
			NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("ARData");
			//Scoreの値が7と一致するオブジェクト検索
			query.OrderByDescending("updateDate");
			query.Limit = 1;
			query.FindAsync ((List<NCMBObject> objList ,NCMBException e) => {
				if (e != null) {
					//検索失敗時の処理
					Debug.Log("Couldn't receive ARData");
				} else {
					//検索成功
					ArrayList worldmapArray = (ArrayList) objList[0]["worldmap"];
					byte[] byteArray = ArrayListToBytes(testArray);
					Debug.Log("Loading worldmap");
					text.text = "Loading worldmap";
					ARWorldMap arWorldMap = ARWorldMap.SerializeFromByteArray(byteArray);
					Debug.Log("Loaded worldmap");
					text.text = "Loaded worldmap ";
					StartSession(arWorldMap);
				}
			});
		*/
		if (!hasStarted) {
			hasStarted = true;
		}
		while (true) {

			yield return new WaitForSeconds (10);
		}
	}

	byte[] ArrayListToBytes(ArrayList array){
		byte[] convertedArray = new byte[array.Count];
		for(int i=0; i<array.Count; i++){
			convertedArray[i] = Convert.ToByte(array[i]);
		}
		return convertedArray;
	}
}
