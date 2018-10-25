using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kakera
{

    //スタンプとコメントと写真を表示するためのもの
    public class StampFunction : MonoBehaviour
    {
        [SerializeField]
        private GameObject Scroll;
        //写真
        [SerializeField]
        private Unimgpicker imagePicker;

		[SerializeField]
		private ARObjectMaker AROM;

        TouchScreenKeyboard keyboard;

        private void Awake()
        {
			/*
			Debug.Log ("stampFunction awake");
            //写真のために読み込んでいる
            imagePicker.Completed += (string path) =>
            {
				StartCoroutine(LoadImageCoroutine(path));
				Debug.Log("Completed");
                LoadImage(path);
            };
            */
        }

        //スタンプを送れるモードにする
        //スタンプが表示されているかどうかを読み取る関数を追加
        private bool mode;
        public void StampButton()
        {
            //スタンプの表示非表示
            mode = Scroll.activeSelf;
            if (mode == false)
            {
                Scroll.SetActive(true);
            }
            if (mode == true)
            {
                Scroll.SetActive(false);
            }
        }
        //個々のスタンプ
        public void OnTapStamp(string id)
        {
         
            string idstamp;
            idstamp = "stamp:" + id;
            AROM.MakeARObject(idstamp);
        }

        //コメント
        public void OnTapComment()
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
            StartCoroutine(ZeroPointOneSecondWait());
        }

        //コメント用のコルーチン
        IEnumerator ZeroPointOneSecondWait()
        {
            while (keyboard.status != TouchScreenKeyboard.Status.Done)
            {
                yield return new WaitForSeconds(0.1f);
            }
            AROM.MakeARObject("comment:" + keyboard.text);
        }


        //写真と動画
        public void OnTapPicture()
        {
            imagePicker.Show("Select Image", "unimagepicker", 1024);
            

        }

        //写真用関数
        private void LoadImage(string path)
        {
			Debug.Log ("loaded image:"+path);
            AROM.MakeARObject("picture:" + path);
        }

		//写真用コルーチン
		private IEnumerator LoadImageCoroutine(string path)
		{
			yield return new WaitForSeconds (3);
			Debug.Log ("pic"+path);
		}
			
    }
}
