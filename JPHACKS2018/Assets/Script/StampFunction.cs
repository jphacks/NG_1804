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
            //写真のために読み込んでいる
            imagePicker.Completed += (string path) =>
            {
                LoadImage(path);
            };
        }

        void MakeARObject(string id)
        {

        }
        //スタンプを送れるモードにする
        public void StampButton()
        {
            Scroll.SetActive(true);
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
			
    }
}
