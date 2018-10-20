using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class ARObjectMaker : MonoBehaviour {

	public GameObject CommentPrefab;
	public GameObject[] StampPrefab;
	public GameObject PicturePrefab;
	public GameObject MoviePrefab;

	public Transform AROBjectParent;

	string lastID;

	void Start(){
		MakeARObject ("stamp:cat");
	}

	public void MakeARObject(string id){
		Vector3 makepos = Camera.main.transform.position;
		Quaternion makerot = Camera.main.transform.rotation;
		GameObject madeObject = MakeObjectFromID (id, makepos, makerot);
		UnityARSessionNativeInterface.GetARSessionNativeInterface ().AddUserAnchorFromGameObject (madeObject);
		lastID = id;
	}

	public GameObject MakeObjectFromID(string id, Vector3 pos, Quaternion rot){
		GameObject madeObject = null;
		GameObject cam = Camera.main.gameObject;
		string[] spids = id.Split (':');
		string type = spids [0];
		string spid = spids [1];
		switch (type) {
		case "comment":
			madeObject = Instantiate (CommentPrefab, pos, rot, AROBjectParent);
			break;
		case "stamp":
			switch (spid) {
			case "cat":
				madeObject = Instantiate (StampPrefab[0], pos, rot, AROBjectParent);
				return StampPrefab [0];
				break;
			}
			break;
		case "picture":
			string[] spspids = spid.Split ('.');
			string extention = spspids [1];
			string filepath = spspids [0];
			if (extention == "MOV" || extention == "mov") {
				madeObject = Instantiate (PicturePrefab, pos, rot, AROBjectParent);
				return MoviePrefab;
			} else {
				madeObject = Instantiate (MoviePrefab, pos, rot, AROBjectParent);
				return PicturePrefab;
			}
			break;
		}
		return madeObject;
	}
}
