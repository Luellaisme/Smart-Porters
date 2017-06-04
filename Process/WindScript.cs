using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindScript : MonoBehaviour {
    
    MoveChestScript moveChest;    
    Text bfScale;
    Text windSpeed;
    public static GameObject windInfo;
    public static bool isWindChange = false;

 
    // Use this for initialization
    void Start () {
        bfScale = GameObject.Find("BeaufortScale").GetComponent<Text>();
        windSpeed = GameObject.Find("WindSpeed").GetComponent<Text>();
        windInfo = GameObject.Find("WindInfo");

        moveChest = new MoveChestScript();
    }

    // Update is called once per frame
    void Update () {
        if (SPWholeVar.isScdBegin)
        {
            SPWholeVar.fstLevel.SetActive(false);
            SPWholeVar.scdLevel.SetActive(true);
            if (SPWholeVar.canChestMove)
            {
                //移动物体
                moveChest.MoveChest();
            }
            if (SPWholeVar.isCeleOver)    //
            {
                SPWholeVar.isScdBegin = false;
                SPWholeVar.isThrdBegin = true;
                
                SPWholeVar.toThisEnd = false;
                SPWholeVar.isCeleOver = false;
            }

            windSpeed.text = (Serial.windY*3.6f).ToString("#.00");
            //判断毕福特风级
            if (Serial.windY < 0.3f)
                bfScale.text = "0";
            else if (Serial.windY >= 0.3f && Serial.windY < 1.6f)
                bfScale.text = "1";
            else if (Serial.windY >= 1.6f && Serial.windY < 3.4f)
                bfScale.text = "2";
            else if (Serial.windY >= 3.4 && Serial.windY < 5.5)
                bfScale.text = "3";
            else if (Serial.windY >= 5.5 && Serial.windY < 8)
                bfScale.text = "4";
            else if (Serial.windY >= 8 && Serial.windY < 10.8)
                bfScale.text = "5";
            else if (Serial.windY >= 10.8 && Serial.windY < 13.9)
                bfScale.text = "6";
            else if (Serial.windY >= 13.9 && Serial.windY < 17.2)
                bfScale.text = "7";
            else if (Serial.windY >= 17.2 && Serial.windY < 20.8)
                bfScale.text = "8";
            else if (Serial.windY >= 20.8 && Serial.windY < 24.5)
                bfScale.text = "9";
            else if (Serial.windY >= 24.5 && Serial.windY < 28.5)
                bfScale.text = "10";
            else if (Serial.windY >= 28.5 && Serial.windY < 32.6f)
                bfScale.text = "11";
            //else if (Serial.windY >= 119f && Serial.windY < 135f)
            //    bfScale.text = "12";
            //else if (Serial.windY >= 135f && Serial.windY < 159f)
            //    bfScale.text = "13";
            //else if (Serial.windY >= 159f && Serial.windY < 167f)
            //    bfScale.text = "14";
            //else if (Serial.windY >= 167f && Serial.windY < 185f)
            //    bfScale.text = "15";
            //else if (Serial.windY >= 185f && Serial.windY < 198f)
            //    bfScale.text = "16";
            //else if (Serial.windY >= 198f)
            //    bfScale.text = "17";
        }
    }

}
