using UnityEngine;
using System.Collections;

namespace Kakera
{
    public class PickerController : MonoBehaviour
    {
        [SerializeField]
        private Unimgpicker imagePicker;

        

        void Awake()
        {
            imagePicker.Completed += (string path) =>
            {
                StartCoroutine(LoadImage(path));
            };
        }

        public void OnPressShowPicker()
        {
            //カメラロール
            imagePicker.Show("Select Image", "unimgpicker", 1024);
        }

        private IEnumerator LoadImage(string path)
        {
            var url = "file://" + path;
            var www = new WWW(url);
            yield return www;

            var texture = www.texture;
            if (texture == null)
            {
                Debug.LogError("Failed to load texture url:" + url);
            }

           // output.material.mainTexture = texture;
        }



    }
}