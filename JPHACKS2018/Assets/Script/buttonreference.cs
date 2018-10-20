using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonreference : MonoBehaviour {
    public GameObject StampButton;
    public GameObject ImageButton;
    public GameObject CommentButton;
    public bool mode;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
            mode = StampButton.activeSelf;
        }
        public void OnClick()
        {
        if (mode == true)
        {
            StampButton.SetActive(false);
            ImageButton.SetActive(false);
            CommentButton.SetActive(false);
        }
         if (mode == false)
        {
            StampButton.SetActive(true);
            ImageButton.SetActive(true);
            CommentButton.SetActive(true);
        }
        }
    
}
