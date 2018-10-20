using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonreference : MonoBehaviour {
    public GameObject MainButton;
    public bool mode;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
            mode = MainButton.activeSelf;
        }
        public void OnClick()
        {
        if (mode == true)
        {
            MainButton.SetActive(false);
        }
         
        }
    
}
