using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseControl : MonoBehaviour {

    GameObject horseWalk;
    GameObject horseRun;
    GameObject horseRun1;
    Transform terrain;
    // Use this for initialization
    void Start () {
        horseWalk = GameObject.Find("Horse Walk");
        horseRun = GameObject.Find("Horse Run");
        horseRun1 = GameObject.Find("Horse Run1");
        terrain = GameObject.Find("Terrain_Guide").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update () {
        if (horseWalk.transform.position.x <= terrain.transform.position.x + 200f)
            horseWalk.transform.position += new Vector3(0.03f, 0, 0);
        else horseWalk.transform.position = new Vector3(terrain.transform.position.x , horseWalk.transform.position.y, horseWalk.transform.position.z);
        if (horseRun.transform.position.x >= terrain.transform.position.x )
            horseRun.transform.position -= new Vector3(0.2f, 0, 0);
        else horseRun.transform.position = new Vector3(terrain.transform.position.x+200f, horseRun.transform.position.y, horseRun.transform.position.z);
        if (horseRun1.transform.position.x >= terrain.transform.position.x)
            horseRun1.transform.position -= new Vector3(0.2f, 0, 0);
        else horseRun1.transform.position = new Vector3(terrain.transform.position.x + 200f, horseRun1.transform.position.y, horseRun1.transform.position.z);
    }
}
