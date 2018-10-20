using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMove : MonoBehaviour {
    public GameObject player;

    private int timer = 0;

    private void Start()
    {
        StartCoroutine(DelayMethod(0.1f));
    }
    // Update is called once per frame
    void Update () {
        Transform mytransform = player.transform;
        Vector3 Pos = mytransform.position;
        if (Pos.x <= 70 && timer == 1)
        {       transform.position += new Vector3(0.4f, 0.4f, 0);
                transform.Rotate(new Vector3(-0.5f, 0, 0));
        }
	}

    IEnumerator DelayMethod(float time)
    {
        yield return new WaitForSeconds(time);
        timer += 1;
    }
}
