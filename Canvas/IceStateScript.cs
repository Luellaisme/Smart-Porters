using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class IceStateScript : MonoBehaviour {
    GameObject icePic;
    public Sprite KylinS1;
    public Sprite KylinS2;
    public Sprite KylinS3;
    public Sprite KylinS4;
    public Sprite KylinS5;

    public Texture KylinT;
    public Texture KylinT1;
    public Texture KylinT2;
    public Texture KylinT3;
    public Texture KylinT4;
    
    int i = 1;
    public float speed = 0;

	// Use this for initialization
	void Start () {
        icePic=GameObject.Find("Ice State");
        speed = 5*1f / SPWholeVar.timeTotal ;

        icePic.GetComponent<RawImage>().texture = KylinT;
        icePic.GetComponent<SpriteRenderer>().sprite = KylinS1;

	    icePic.GetComponent<SpriteRenderer>().color = new Vector4(255, 255, 255, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (SPWholeVar.isGameBegin)
        {
            if (icePic.GetComponent<SpriteRenderer>().color.a < 1)
                icePic.GetComponent<SpriteRenderer>().color = new Vector4(255, 255, 255, icePic.GetComponent<SpriteRenderer>().color.a + speed * Time.deltaTime );
            else
            {
                i = i + 1;
                icePic.GetComponent<SpriteRenderer>().color = new Vector4(255, 255, 255, 0);
            }
            switch(i)
            {
                case 1:
                    icePic.GetComponent<RawImage>().texture = KylinT;
                    icePic.GetComponent<SpriteRenderer>().sprite = KylinS1;
                    break;
                case 2:
                    icePic.GetComponent<RawImage>().texture = KylinT1;
                    icePic.GetComponent<SpriteRenderer>().sprite = KylinS2;
                    break;
                case 3:
                    icePic.GetComponent<RawImage>().texture = KylinT2;
                    icePic.GetComponent<SpriteRenderer>().sprite = KylinS3;
                    break;
                case 4:
                    icePic.GetComponent<RawImage>().texture = KylinT3;
                    icePic.GetComponent<SpriteRenderer>().sprite = KylinS4;
                    break;
                case 5:
                    icePic.GetComponent<RawImage>().texture = KylinT4;
                    icePic.GetComponent<SpriteRenderer>().sprite = KylinS5;
                    break;
                default:
                    break;
            }
        }
        }  
    
}
