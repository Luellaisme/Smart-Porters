using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationDevice : MonoBehaviour {
    public GameObject calibration;
    public RawImage calibCircle;

    float timeStep = 0.5f;

    IEnumerator ChngColor()
    {
        while (SPWholeVar.isCalibrate)
        {
            if (calibration)
            {
                if (calibCircle.color.a == 1)
                    calibCircle.color = new Vector4(255, 255, 255, 0);
                else
                    calibCircle.color = new Vector4(255, 255, 255, 1);
            }
            yield return new WaitForSeconds(timeStep);
        }
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(ChngColor());
        }	

    public void OnCalibration()
    {
        //SPWholeVar.isGameBegin = true;
        GameObject.Find("Character").GetComponent<Serial>().startGame();
        SPWholeVar.isCalibrate = false;
        SPWholeVar.isPrevOn = true;        
    }

    // Update is called once per frame
    void Update () {
	}
}
