using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.UI;
using UnityEngine.Video;

public class ARObjectMaker : MonoBehaviour {

	public GameObject CommentPrefab;
	public GameObject[] StampPrefab;
	public GameObject PicturePrefab;
	public GameObject MoviePrefab;

	public Transform AROBjectParent;

	public string lastID;
	public GameManager GM;
	public AREventManager AREM;

	public bool nowMade = false;

	void Start(){
		MakeARObject ("stamp:cat");
	}

	//makeARObject with anchor
	public void MakeARObject(string id){
		if (GM.hasStarted) {
			Quaternion makerot = Camera.main.transform.rotation;
			Vector3 makepos = Camera.main.transform.position + 0.2f * (makerot * Vector3.forward);
			GameObject madeObject = MakeObjectFromID (id, makepos, makerot);
			UnityARSessionNativeInterface.GetARSessionNativeInterface ().AddUserAnchorFromGameObject (madeObject);
			lastID = id;
			nowMade = true;
			Debug.Log ("GameObject is Made with Anchor");
		}
	}

	public GameObject MakeObjectFromID(string id, Vector3 pos, Quaternion rot, bool fromLoad = false){
		GameObject madeObject = null;
		GameObject cam = Camera.main.gameObject;
		string[] spids = id.Split (':');
		string type = spids [0];
		string spid = spids [1];
		switch (type) {
		case "comment":
			madeObject = Instantiate (CommentPrefab, pos, rot, AROBjectParent);
			madeObject.GetComponent<TextMesh> ().text = spid;
			break;
		case "stamp":
			switch (spid) {
			case "hata":
				madeObject = Instantiate (StampPrefab[0], pos, rot, AROBjectParent);
				break;
			case "JP":
				madeObject = Instantiate (StampPrefab[1], pos, rot, AROBjectParent);
				break;
			case "fish1":
				madeObject = Instantiate (StampPrefab[2], pos, rot, AROBjectParent);
				break;
			case "fish2":
				madeObject = Instantiate (StampPrefab[3], pos, rot, AROBjectParent);
				break;
			case "fish3":
				madeObject = Instantiate (StampPrefab[4], pos, rot, AROBjectParent);
				break;
			}
			break;
		case "picture":
			if (!fromLoad) {
				Debug.Log ("path:"+spid);
				if (spids.Length == 3) {
					madeObject = Instantiate (MoviePrefab, pos, rot, AROBjectParent);
					madeObject.GetComponent<VideoPlayer> ().url = spids[1] + ":" + spids[2];
					madeObject.GetComponent<VideoPlayer> ().Play ();
					Debug.Log ("Movie is Played");
				} else {
					madeObject = Instantiate (PicturePrefab, pos + 0.7f * (Camera.main.transform.rotation * Vector3.forward), rot, AROBjectParent);
					MeshRenderer output = madeObject.GetComponent<MeshRenderer> ();
					StartCoroutine (SetPicture (spid, output));
				}
			}
			break;
		}
		return madeObject;
	}

	IEnumerator SetPicture(string path, MeshRenderer output){
		var url = "file://" + path;
		var www = new WWW(url);
		yield return www;

		var texture = www.texture;
		if (texture == null)
		{
			Debug.LogError("Failed to load texture url:" + url);
		}

		output.material.mainTexture = texture;
		Debug.Log ("Success to SetPicture");
	}
}
