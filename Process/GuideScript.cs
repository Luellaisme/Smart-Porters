using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideScript : MonoBehaviour {
    
    MoveChestScript moveChest;

    GameObject guide1;
    GameObject guide2;
    GameObject guide3;

    RawImage circle;
    GameObject finger;
    GameObject arrow;

    bool fingerMov;
    bool canChestMove;
    Vector3 fingerPos;

    float timeStep = 0.5f;

    // Use this for initialization
    void Start () {
        moveChest = new MoveChestScript();
        //moveChest = gameObject.GetComponent<MoveChestScript>();

        guide1 = GameObject.Find("Guide1");
        guide2 = GameObject.Find("Guide2");
        guide3 = GameObject.Find("Guide3");
        circle = GameObject.Find("Circle").GetComponent<RawImage>();
        finger = GameObject.Find("Finger");
        arrow = GameObject.Find("Arrow");
        
        guide2.SetActive(false);
        guide3.SetActive(false);

        fingerMov = false;

        StartCoroutine(ChngColor());
    }

    //确认Haply归位
    public void OnTargClick()
    {
        guide1.SetActive(false);
        guide2.SetActive(true);

        fingerPos = finger.transform.position;
        fingerMov = true;
    }

    //手指移动动画
    void FingerMove()
    {
        Vector3 v = fingerPos;
        v.x += 2 * (arrow.transform.position.x - fingerPos.x) * Mathf.Abs(Mathf.Sin(Time.time * 1.0f));
        finger.transform.position = v;
        SPWholeVar.canChestMove = true;
        //if (Serial.touchChest)    //设置停止提示的条件
        //{
        //    fingerMov = false;
        //   
        //}
    }

    IEnumerator ChngColor()
    {
        while ( SPWholeVar.isGuide && guide1)
        {
            if (circle)
            {
                if (circle.color.a == 1)
                    circle.color = new Vector4(255, 255, 255, 0);
                else
                    circle.color = new Vector4(255, 255, 255, 1);
            }
            yield return new WaitForSeconds(timeStep);
        }
    }

    // Update is called once per frame
    void Update () {
        if(SPWholeVar.isGuide)
        {
            if (fingerMov)
            {
                arrow.SetActive(true);
                finger.SetActive(true);
                FingerMove();
                moveChest.MoveChest();
            }
            
            //else if(SPWholeVar.canChestMove)
            //{
            //    arrow.SetActive(false);
            //    finger.SetActive(false);
            //    
            //}
            if (SPWholeVar.toThisEnd )
            {
                guide2.SetActive(false);
                guide3.SetActive(true);
            }
        }
    }

    //游戏开始
    public void OnBeginClick()
    {
        SPWholeVar.isGuide = false;
        SPWholeVar.isGameBegin = true;
        SPWholeVar.isFstBegin = true;
        
        SPWholeVar.toThisEnd = false;
        SPWholeVar.isCeleOver = false;
    }

    //游戏开始
    public void OnSkipClick()
    {
        SPWholeVar.isGuide = false;
        SPWholeVar.canChestMove = false;

        SPWholeVar.isGameBegin = true;
        SPWholeVar.isFstBegin = true;
        SPWholeVar.toThisEnd = false;
        SPWholeVar.isCeleOver = false;
    }
}
