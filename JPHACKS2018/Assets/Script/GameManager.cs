using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using UnityEngine.XR.iOS;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {
	private UnityARSessionNativeInterface m_session;
	[Header("AR Config Options")]
	public UnityARAlignment startAlignment = UnityARAlignment.UnityARAlignmentGravity;
	public UnityARPlaneDetection planeDetection = UnityARPlaneDetection.Horizontal;
	public ARReferenceImagesSet detectionImages = null;
	public bool getPointCloud = true;
	public bool enableLightEstimation = true;
	public bool enableAutoFocus = true;
	private bool sessionStarted = false;

	public AREventManager AREM;

	enum GameState{
		Idle, WorldMapInit, Playering
	}
	string[] madeObjectIDs;
	public Dictionary<string, string> dicIDs = new Dictionary<string, string>();
	public bool hasStarted = false;
	GameState state = GameState.Idle;
	public LocationUpdater LU;

	// Use this for initialization
	void Start () {

	}

	public void AddDicIDs(string anchorid, string objectid){
		if (!dicIDs.ContainsKey (anchorid)) {
			dicIDs.Add (anchorid, objectid);
		}
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
		Debug.Log ("DataLoadStart");
		while (LU.Location.latitude == 0) {
			Debug.Log (LU.Location.latitude);
			yield return new WaitForSeconds (0.5f);
		}
			//QueryTestを検索するクラスを作成
			NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("ARData");
			//Scoreの値が7と一致するオブジェクト検索
			//query.OrderByDescending("updateDate");
		//in nagoya test
		//NCMBGeoPoint geo = new NCMBGeoPoint (35.1522, 136.9525);
		NCMBGeoPoint geo = new NCMBGeoPoint (LU.Location.latitude, LU.Location.longitude);
		Debug.Log ("lat" + LU.Location.latitude);
		Debug.Log ("lat" + LU.Location.longitude);
		query.WhereGeoPointWithinKilometers ("point", geo, 10);
			//query.Limit = 1;
			query.FindAsync ((List<NCMBObject> objList ,NCMBException e) => {
				if (e != null) {
					//検索失敗時の処理
					Debug.Log("Couldn't receive ARData");
					Debug.Log(e.ErrorMessage);
				} else {
					//検索成功
				Debug.Log("Success Find count:"+objList.Count);
					ArrayList worldmapArray = (ArrayList) objList[0]["worldmap"];
					byte[] byteArray = ArrayListToBytes(worldmapArray);
					Debug.Log("Loading worldmap");
					ARWorldMap arWorldMap = ARWorldMap.SerializeFromByteArray(byteArray);
					Debug.Log("Loaded worldmap");
					StartSession(arWorldMap);
					Dictionary<string, object> loadDic = (Dictionary<string, object>) objList[0]["IDPair"];
					Dictionary<string, string> sd = new Dictionary<string, string>(); 
					foreach (KeyValuePair<string, object> keyValuePair in loadDic)
					{
						sd.Add(keyValuePair.Key, keyValuePair.Value.ToString());
					}
					dicIDs = new Dictionary<string, string>(sd);
					AREM.usingDataID = objList[0].ObjectId;
				}
			});

		if (!hasStarted) {
			hasStarted = true;
		}
		while (true) {

			yield return new WaitForSeconds (10);
		}
	}

	//Load
	public void StartSession(ARWorldMap arWorldMap = null)
	{
		m_session = UnityARSessionNativeInterface.GetARSessionNativeInterface();

		Application.targetFrameRate = 60;
		ARKitWorldTrackingSessionConfiguration config = new ARKitWorldTrackingSessionConfiguration();
		config.planeDetection = planeDetection;
		config.alignment = startAlignment;
		config.getPointCloudData = getPointCloud;
		config.enableLightEstimation = enableLightEstimation;
		config.enableAutoFocus = enableAutoFocus;
		config.worldMap = arWorldMap;
		if (detectionImages != null) {
			config.referenceImagesGroupName = detectionImages.resourceGroupName;
		}

		/*
		UnityARSessionRunOption runOption = UnityARSessionRunOption.ARSessionRunOptionRemoveExistingAnchors |
			UnityARSessionRunOption.ARSessionRunOptionResetTracking;
			*/

		m_session.RunWithConfig (config);
		//m_session.AddUserAnchorFromGameObject

		/*
		if (config.IsSupported) {
			m_session.RunWithConfig (config);
			//UnityARSessionNativeInterface.ARFrameUpdatedEvent += FirstFrameUpdate;
		}
		*/
	}

	byte[] ArrayListToBytes(ArrayList array){
		byte[] convertedArray = new byte[array.Count];
		for(int i=0; i<array.Count; i++){
			convertedArray[i] = Convert.ToByte(array[i]);
		}
		return convertedArray;
	}
}
