using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using NCMB;

public class AREventManager : MonoBehaviour {
	public string usingDataID = "";
	List<string> madeIDs = new List<string> ();
	public ARObjectMaker AROM;
	public GameManager GM;
	public LocationUpdater LU;


	// Use this for initialization
	void Start () {
		UnityARSessionNativeInterface.ARFrameUpdatedEvent += OnFrameUpdate;
		UnityARSessionNativeInterface.ARUserAnchorAddedEvent += OnUserAnchorAdded;
		UnityARSessionNativeInterface.ARUserAnchorUpdatedEvent += OnUserAnchorUpdated;
	}

	public ARWorldMappingStatus lastStatus;
	public void OnFrameUpdate(UnityARCamera cam){
		ARWorldMappingStatus status = cam.worldMappingStatus;
		if (lastStatus != status && status == ARWorldMappingStatus.ARWorldMappingStatusMapped && GM.hasStarted) {
			Save ();
		}
		lastStatus = status;
		Debug.Log (status);
		Debug.Log (cam.trackingReason);
	}

	public void OnUserAnchorAdded(ARUserAnchor anchor){
		GM.AddDicIDs (anchor.identifier, AROM.lastID);
		if (AROM.nowMade) {
			madeIDs.Add (anchor.identifier);
			AROM.nowMade = false;
			Debug.Log ("Object is Made by User");
		} else {
			if (madeIDs.Contains (anchor.identifier)) {
				return;
			} else {
				Vector3 pos = UnityARMatrixOps.GetPosition (anchor.transform);
				Quaternion rot = UnityARMatrixOps.GetRotation (anchor.transform);
				AROM.MakeObjectFromID (GM.dicIDs[anchor.identifier], pos, rot, true);
				madeIDs.Add (anchor.identifier);
				Debug.Log ("GameObject is Made From Anchor");
			}
		}
		Debug.Log ("Anchor is Added");
	}

	public void OnUserAnchorUpdated(ARUserAnchor anchor){
		Debug.Log ("Anchor is Updated id:" + anchor.identifier);

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
			NCMBGeoPoint geoobj = new NCMBGeoPoint (LU.Location.latitude, LU.Location.longitude);
			obj.Add ("point", geoobj);
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
}
