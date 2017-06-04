using System.Collections;
using System.Collections.Generic;
using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class MoveChestScript : MonoBehaviour
{

    Vector3 mousePos;
    float x;
    float y;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void KeepMouseIn()
    {
        x = GameObject.Find("Character").GetComponent<Serial>().x_1;
        y = GameObject.Find("Character").GetComponent<Serial>().y_1;
        mousePos = new Vector3(x, y, 0.0f);
        //print(Input.mousePosition);

    }

    //沿X轴移动宝箱，作用为坐标变换
    public void MoveChest()
    {
        if (SPWholeVar.character && SPWholeVar.chest)
        {
            //KeepMouseIn();

            //移动宝箱
            if (!SPWholeVar.toThisEnd && EnergyScript.hasEnergy)//右键输入+非结束场景+具有能量
            {
                //将宝箱的世界坐标映射到屏幕的像素坐标，得到摄像头到物体的距离。
                float distToScreen = SPWholeVar.curCam.WorldToScreenPoint(SPWholeVar.chest.transform.position).z;

                //鼠标位置输入
                KeepMouseIn();


                //像素坐标映射到世界坐标中，得 到终点的世界坐标。
                Vector3 endPos = SPWholeVar.curCam.ScreenToWorldPoint(new Vector3(SPWholeVar.endPoint * Screen.width, mousePos.y, distToScreen));

                //在终点位置之前
                if (SPWholeVar.chest.transform.position.x <= endPos.x)
                {
                    //Pos得到由鼠标位置的像素坐标转化为的世界坐标
                    Vector3 Pos = SPWholeVar.curCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distToScreen));

                    if (SPWholeVar.isGuide)
                    {
                        SPWholeVar.character.transform.position = new Vector3(Pos.x, SPWholeVar.chest.transform.position.y, SPWholeVar.chest.transform.position.z);
                    }
                    else if (SPWholeVar.isFstBegin)
                    {
                        SPWholeVar.character.transform.position = new Vector3(Pos.x, Pos.y, SPWholeVar.chest.transform.position.z);

                    }
                    else if (SPWholeVar.isScdBegin)
                    {
                        SPWholeVar.character.transform.position = new Vector3(Pos.x, SPWholeVar.chest.transform.position.y, Pos.z);
                    }
                    else
                    {
                        //物体将以斜向移动
                        float ches_y = (Pos.x - SPWholeVar.charMountain.transform.position.x) *
                            Mathf.Tan(SPWholeVar.charMountain.transform.rotation.eulerAngles.z / 180f * Mathf.PI) + SPWholeVar.charMountain.transform.position.y;
                        SPWholeVar.character.transform.position = new Vector3(Pos.x, ches_y, SPWholeVar.chest.transform.position.z);
                    }
                }
                else //抵达本关终点，中断移动脚本，进入结束画面
                {
                    SPWholeVar.toThisEnd = true;
                    SPWholeVar.character.transform.position = new Vector3(endPos.x, SPWholeVar.chest.transform.position.y, SPWholeVar.chest.transform.position.z);
                }
            }
            else if (SPWholeVar.toThisEnd)
            {
                //抵达终点时播放欢庆效果
                if (!SPWholeVar.isGuide)
                    toPlayAnim();
                SPWholeVar.canChestMove = false; //
            }
        }
    }

    //播放抵达终点动画
    void toPlayAnim()
    {
        SPWholeVar.passThis.SetActive(true);
        if (SPWholeVar.isThrdBegin)
            GameObject.Find("NextLevel").SetActive(false);
    }

    //单击确认关闭动画
    public void OnClick()
    {
        SPWholeVar.passThis.SetActive(false);
        SPWholeVar.isCeleOver = true;
    }

}
