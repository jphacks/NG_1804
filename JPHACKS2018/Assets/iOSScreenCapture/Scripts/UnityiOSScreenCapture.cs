using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class UnityiOSScreenCapture : MonoBehaviour {

    //キャンバスグループを作る
    [SerializeField]
    private CanvasGroup canvasGroup;
    //写真を撮ったときに光る用の画像
    [SerializeField]
    private Image PhotoFlash;

    private void Start()
    {
        //最初は透明にしておく
        PhotoFlash.color = Color.clear;
    }

    public UnityEvent OnCompleteCapture;
	public UnityEvent OnFailCapture;

    //合体させた

    [DllImport("__Internal")]
    private static extern void _PlaySystemShutterSound();

    [DllImport("__Internal")]
    private static extern void _SendTexture(byte[] textureByte, int length);

    [DllImport("__Internal")]
    private static extern void _RequestCameraPermission();

    [DllImport("__Internal")]
    private static extern void _RequestCameraRollPermission();

    [DllImport("__Internal")]
    private static extern int _HasCameraPermission();

    [DllImport("__Internal")]
    private static extern int _HasCameraRollPermission();

    [DllImport("__Internal")]
    private static extern void _GoToSettings();

    public static void PlaySystemShutterSound()
    {
        _PlaySystemShutterSound();
    }

    public static void SaveTexture(byte[] textureByte, int length)
    {
        _SendTexture(textureByte, length);
    }
    public static void RequestPermissions()
    {
        AVAuthorizationStatus avstatus = HasCameraPermission();
        PHAuthorizationStatus phstatus = HasCameraRollPermission();

        //アクセス許可のリクエストを出していない場合はリクエストを送る
        if (avstatus == AVAuthorizationStatus.NotDetermined)
        {
            _RequestCameraPermission();
        }

        if (phstatus == PHAuthorizationStatus.NotDetermined)
        {
            _RequestCameraRollPermission();
        }
    }


    public void Execute() {
        //合体させた
        RequestPermissions();



#if !UNITY_EDITOR
		PHAuthorizationStatus phstatus = (PHAuthorizationStatus)Enum.ToObject(
			typeof(PHAuthorizationStatus), UnityiOS.HasCameraRollPermission());
		UnityiOS.PlaySystemShutterSound();
		if(phstatus == PHAuthorizationStatus.Authorized) {
			Handheld.SetActivityIndicatorStyle(UnityEngine.iOS.ActivityIndicatorStyle.Gray);
			Handheld.StartActivityIndicator();
			StartCoroutine(_CaptureScreenShot());
		} else {
			OnFailCapture.Invoke();
		}
#endif
    }

	private IEnumerator _CaptureScreenShot() {
		canvasGroup.alpha = 0; //みたいな処理を入れておくと撮影時にUIを外すといった事が出来ます
        //画面が光るようにする　引数はRGBA
        PhotoFlash.color = new Color(1f, 1f, 1f, 0.9f);
        StartCoroutine(Flash());
        yield return new WaitForEndOfFrame();

		var width = Screen.width;
		var height = Screen.height;
		var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();
        

        byte[] screenshot = tex.EncodeToPNG();

		UnityiOS.SaveTexture(screenshot, screenshot.Length);

		canvasGroup.alpha = 1;
	}

	//撮影後コールバックされる関数
	void DidImageWriteToAlbum(string errorDescription) {
		Handheld.StopActivityIndicator();
        
        if (string.IsNullOrEmpty(errorDescription)) {
			OnCompleteCapture.Invoke();
		}else{
			OnFailCapture.Invoke();
		}
	}


    /*合体させた部分*/
    public static AVAuthorizationStatus HasCameraPermission()
    {
#if !UNITY_EDITOR
		return (AVAuthorizationStatus)Enum.ToObject(
   				typeof(AVAuthorizationStatus), _HasCameraPermission());
#endif
        return AVAuthorizationStatus.Authorized;
    }

    public static PHAuthorizationStatus HasCameraRollPermission()
    {
#if !UNITY_EDITOR
        return (PHAuthorizationStatus)Enum.ToObject(
				typeof(PHAuthorizationStatus), _HasCameraRollPermission());
#endif
        return PHAuthorizationStatus.Authorized;
    }

    public static void GoToSettings()
    {
#if !UNITY_EDITOR
        _GoToSettings();
#endif
    }

    public void GoToSettings_forUGUI()
    {
#if !UNITY_EDITOR
        _GoToSettings();
#endif
    }

    /*合体させた部分*/

    //カメラのフラッシュのためのコルーチン
    IEnumerator Flash()
    {
        yield return new WaitForSeconds(0.8f);
        PhotoFlash.color = new Color(0f, 0.0f, 0f, 0f);
    }

}
