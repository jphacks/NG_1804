using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//スタンプとコメントと写真を表示するためのもの
public class StampFunction : MonoBehaviour
{

    TouchScreenKeyboard keyboard;

    void MakeARObject(string id)
    {

    }
    //スタンプ
    public void OnTapStamp(string id)
    {
        string idstamp;
        idstamp = "stamp:" + id;
        //MakeARObject(idstamp);
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
        MakeARObject("comment:" + keyboard.text);
    }


    //写真と動画
    public void OnTapPicture()
    {
        
        //MakeARObject("photo:" + );


    }

}
