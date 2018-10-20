using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using NCMB;

public class AREventManager : MonoBehaviour {
	private UnityARSessionNativeInterface m_session;
	[Header("AR Config Options")]
	public UnityARAlignment startAlignment = UnityARAlignment.UnityARAlignmentGravity;
	public UnityARPlaneDetection planeDetection = UnityARPlaneDetection.Horizontal;
	public ARReferenceImagesSet detectionImages = null;
	public bool getPointCloud = true;
	public bool enableLightEstimation = true;
	public bool enableAutoFocus = true;
	private bool sessionStarted = false;

	string usingDataID = "";
	List<string> madeIDs = new List<string> ();
	public ARObjectMaker AROM;
	public GameManager GM;
	public LocationUpdater LU;


	// Use this for initialization
	void Start () {
		UnityARSessionNativeInterface.ARFrameUpdatedEvent += OnFrameUpdate;
		UnityARSessionNativeInterface.ARUserAnchorAddedEvent += OnUserAnchorAdded;
	}

	void Update(){
		Debug.Log(LU.Location.latitude);
		Debug.Log(LU.Location.longitude);
	}

	public ARWorldMappingStatus lastStatus;
	public void OnFrameUpdate(UnityARCamera cam){
		ARWorldMappingStatus status = cam.worldMappingStatus;
		if (lastStatus != status && status == ARWorldMappingStatus.ARWorldMappingStatusMapped) {
			Save ();
		}
		lastStatus = status;
	}

	public void OnUserAnchorAdded(ARUserAnchor anchor){
		GM.AddDicIDs (anchor.identifier, AROM.lastID);
		madeIDs.Add (anchor.identifier);
		Debug.Log ("Anchor is Added");
	}

	public void OnUserAnchorUpdated(ARUserAnchor anchor){
		if (GM.dicIDs.ContainsKey (anchor.identifier)) {
			return;
		} else {
			AROM.MakeObjectFromID (anchor.identifier, Vector3.zero, Quaternion.identity);
			Debug.Log ("GameObject is Made From Anchor");
		}
	}

	public void Save()
	{
		UnityARSessionNativeInterface.GetARSessionNativeInterface ().GetCurrentWorldMapAsync (WorldMapCreated);
	}
	//Save
	void WorldMapCreated(ARWorldMap worldmap){
		if (worldmap != null) {
			NCMBObject obj = new NCMBObject ("ARData");
			if (usingDataID != "")
				obj.ObjectId = usingDataID;
			obj.Add ("worldmap", worldmap.SerializeToByteArray());
			obj.Add ("IDPair", GM.dicIDs);
			obj.SaveAsync ((NCMBException e) => {      
				if (e != null) {
					//エラー処理
					Debug.Log("Map Save Error");
				} else {
					//成功時の処理
					usingDataID = obj.ObjectId;
					Debug.Log("Data is Saved");
				}                   
			});
		} else {
			Debug.Log ("Data is Null");
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
}
