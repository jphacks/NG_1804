using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StampFunction : MonoBehaviour
{

    TouchScreenKeyboard keyboard;

    void MakeARObject(string id)
    {

    }
    //スタンプ
    void OnTapStamp(string id)
    {
        string idstamp;
        idstamp = "stamp:" + id;
        //MakeARObject(idstamp);
    }

    //コメント
    void OnTapComment()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        StartCoroutine(ZeroPointOneSecondWait());
    }
    //写真と動画
    void OnTapPicture()
    {
        //カメラロールの出現


    }

    //コメント用のコルーチン
    IEnumerator ZeroPointOneSecondWait()
    {
        while (keyboard.status != TouchScreenKeyboard.Status.Done)
        {
            yield return new WaitForSeconds(0.1f);
        }
        MakeARObject("comment" + keyboard.text);
    }
}
