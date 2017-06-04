using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterScript : MonoBehaviour {

    MoveChestScript moveChest;

    public GameObject Smerage;
    Text submergedHeight;

    // Use this for initialization
    void Start()
    {
        moveChest = new MoveChestScript();
        //submergedHeight = GameObject.Find("SubmergedHeight").GetComponent<Text>();
        submergedHeight = Smerage.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //submergedHeight = GameObject.Find("SubmergedHeight").GetComponent<Text>();
        if (SPWholeVar.isFstBegin)
        {
            SPWholeVar.fstLevel.SetActive(true);
            if (SPWholeVar.canChestMove)
            {
                moveChest.MoveChest();
            }
            if (SPWholeVar.isCeleOver)    //
            {
                SPWholeVar.isFstBegin = false;
                SPWholeVar.isScdBegin = true;

                AnnCanvasScript.placeCom.SetActive(true);
                SPWholeVar.toThisEnd = false;
                SPWholeVar.isCeleOver = false;
            }

            submergedHeight.text = ((Serial.waterDeep) / 100f).ToString("#.00");
        }
        //else
        //{
        //    SPWholeVar.fstLevel.SetActive(false);
        //    Debug.Log("close");
        //}
    }
    
}

