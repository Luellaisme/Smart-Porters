using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SPWorkFlow : MonoBehaviour {
    GameObject frontCam;
    GameObject topCam;
    Text topViewTitle;

    void Awake()
    {
        frontCam = GameObject.Find("Firstview_Front Camera");
        topCam = GameObject.Find("Firstview_Top Camera");
        topViewTitle = GameObject.Find("Topview Title").GetComponent<Text>();
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        //开始校准
        if (SPWholeVar.isCalibrate)
            SPWholeVar.calibration.SetActive(true);
        else
            SPWholeVar.calibration.SetActive(false);

        ////游戏开始，前情回顾
        //if (SPWholeVar.isGameBegin)
        //    SPWholeVar.prevOn.SetActive(true);
        //else SPWholeVar.prevOn.SetActive(false);
        if (SPWholeVar.isPrevOn)
        {
            SPWholeVar.isGuide = true;
            SPWholeVar.isPrevOn = false;
            SPWholeVar.character.SetActive(true);
        }

        //游戏指导
        if (SPWholeVar.isGuide)
        {
            SPWholeVar.curCam = SPWholeVar.camGuide;
            SPWholeVar.terrainGuide.SetActive(true);
            SPWholeVar.guide.SetActive(true);
        }
        else
        {
            SPWholeVar.terrainGuide.SetActive(false);
            SPWholeVar.guide.SetActive(false);
        }

        //第一关开始
        if (SPWholeVar.isFstBegin)
        {
            SPWholeVar.terrainWater.SetActive(true);
            SPWholeVar.curCam = SPWholeVar.camWater;
            CharPosChng(SPWholeVar.charWater);
        }
        else
        {
            SPWholeVar.terrainWater.SetActive(false);
            SPWholeVar.scdLevel.SetActive(false);
        }

        //第二关开始
        if (SPWholeVar.isScdBegin)
        {
            SPWholeVar.terrainWind.SetActive(true);
            SPWholeVar.curCam = SPWholeVar.camWind;
            CharPosChng(SPWholeVar.charWind);

            topCam.SetActive(false);
            topViewTitle.text = "Front View";
        }
        else
        {
            SPWholeVar.terrainWind.SetActive(false);
            SPWholeVar.fstLevel.SetActive(false);

            topCam.SetActive(true);
            topViewTitle.text = "Top View";
        }

        //第三关开始
        if (SPWholeVar.isThrdBegin)
        {
            SPWholeVar.terrainMountain.SetActive(true);
            SPWholeVar.curCam = SPWholeVar.camMountain;
            CharPosChng(SPWholeVar.charMountain);
        }
        else
        {
            SPWholeVar.terrainMountain.SetActive(false);
            SPWholeVar.thrdLevel.SetActive(false);
        }

        //游戏结束
        if (SPWholeVar.isLose || SPWholeVar.isSuccess)
        {
            SPWholeVar.terrainWind.SetActive(false);
            SPWholeVar.terrainWater.SetActive(false);
            SPWholeVar.terrainMountain.SetActive(false);
            //SPWholeVar.character.SetActive(false);
            SPWholeVar.character.transform.position = SPWholeVar.charFinal.transform.position;
            SPWholeVar.curCam = null;
            if(GameObject.Find("Topview Image"))
            {
                GameObject.Find("Topview Image").SetActive(false);
                GameObject.Find("Backview Image").SetActive(false);
            }
        }
    }

    //更替Character位置
    void CharPosChng(GameObject thisChara)
    {
        if (!SPWholeVar.canChestMove && !SPWholeVar.toThisEnd)
        {
            SPWholeVar.character.transform.position = thisChara.transform.position;
        }
    }

    //退出
    public void OnClose()
    {
        Application.Quit();
    }
}
