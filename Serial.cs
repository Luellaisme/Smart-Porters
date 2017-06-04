using System.Collections;
using System.Collections.Generic;
using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class Serial : MonoBehaviour
{

    SerialPort sp = new SerialPort("COM1", 115200);
    Caualation Caculate = new Caualation();
    private int activeNumber = 2;

    private byte deviceAddress = 0x41;
    private float resoluation = 13824;

    private byte[] receiveData;
    private byte[] outData;
    private byte command = 0x03;
    private byte command1 = 0x13;

    private float[] angles = { 0, 0 };
    private float[] force = { 0, 0 };
    private float[] torque = { 0, 0 };

    private float[] position = { 0, 0 };  //此时接收的位置
    private float[] position0 = { 0, 0 }; //起始时刻的位置
    private float[] position1 = { 0, 0 }; //上一时刻的位置

    private Vector2 V_a = new Vector2(0, 0);  //真实移动向量
    private Vector2 V_p = new Vector2(0, 0);  //预测移动向量

    private float xForce1, xForce2, yForce1, yForce2;
    private float xVelocity, yVelocity;

    //木箱坐标(屏幕坐标)
    public float x_1 = 0;
    public float y_1 = 0;

    //触点坐标（屏幕坐标）
    private float x_0 = 0;
    private float y_0 = 0;

    //触点与物体屏幕坐标差值
    private float x_d = 0;
    private float y_d = 0;

    private int t = 1;
    public static int k = 3;
    private float edge = 20f;
    private float C = 10000f;

    //商议时刻位置
    private float x_last = 0f;
    private float y_last = 0f;

    Toggle mountainWood;
    Toggle mountainMental;
    Toggle mountainRock;
    Toggle mountainGlass;


    //界面调用参数
    public static float windY;
    public static float waterDeep;
    public static float energyForce;

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    struct uFloat
    {
        [FieldOffset(0)]
        public long val_l;
        [FieldOffset(0)]
        public float val_f;
    }

    // Use this for initialization
    void Start()
    {
        mountainWood = GameObject.Find("Wood").GetComponent<Toggle>();
        mountainMental = GameObject.Find("Mental").GetComponent<Toggle>();
        mountainRock = GameObject.Find("Rock").GetComponent<Toggle>();
        mountainGlass = GameObject.Find("Glass").GetComponent<Toggle>();
    }

    public void startGame()
    {
        print("1");
        receiveData = new byte[4 * activeNumber];
        outData = new byte[1 + 4 * 2 * activeNumber];
        outData[0] = deviceAddress;

        sp.Open();
        sp.ReadTimeout = 10;
        sp.WriteTimeout = 10;

        DeviceStartFunction(180, 360);
        angles[0] = 180;
        angles[1] = 360;
        position0 = Caculate.GetPosition(angles);
    }
    // Update is called once per frame
    void Update()
    {
        print(x_0 + " " + y_0);
        //print(Input.mousePosition);
        //print((position[0] - position0[0]) * 300 + "  " + (position[1] - position0[1]) * 300);
        //print(force[0] + "  " + force[1]);
    }

    //结束程序
    void onApplicationQuit()
    {
        force[0] = force[1] = 0;
        torque = GetTorque(force);
        SetTorque(torque);
        GetDegree();
        position = Caculate.GetPosition(angles);
        sp.Close();
    }

    // FixedUpdate is called once per 0.002s
    void FixedUpdate()
    {

        //每关初始化
        if (SPWholeVar.isGuide == true && t == 1)
        {
            t = 0;
            x_1 = SPWholeVar.curCam.WorldToScreenPoint(SPWholeVar.chest.transform.position).x;
            y_1 = SPWholeVar.curCam.WorldToScreenPoint(SPWholeVar.chest.transform.position).y;
            force[0] = 0;
            force[1] = 0;
        }
        else if (SPWholeVar.isFstBegin == true && t == 0)
        {
            t = 1;
            x_1 = 720f / k;
            y_1 = 1200f / k;
            force[0] = 0;
            force[1] = 0;
        }
        else if (SPWholeVar.isScdBegin == true && t == 1)
        {
            t = 0;
            x_1 = 750f / k;
            y_1 = 1050f / k;
            force[0] = 0;
            force[1] = 0;
            
        }
        else if (SPWholeVar.isThrdBegin == true && t == 0)
        {
            t = 1;
            x_1 = 660f / k;
            y_1 = 900f / k;
            force[0] = 0;
            force[1] = 0;
        }

        x_0 = (-40.0f * (position[0] - position0[0]) * 300 + 1368) / k;
        y_0 = (-35.5f * (position[1] - position0[1]) * 300 + 1580) / k;

        try
        {
            x_1 = SPWholeVar.curCam.WorldToScreenPoint(SPWholeVar.chest.transform.position).x;
            y_1 = SPWholeVar.curCam.WorldToScreenPoint(SPWholeVar.chest.transform.position).y;
        }
        catch (Exception ex) { }

        x_d = x_0 - x_1;
        y_d = y_0 - y_1;

        //速度计算
        //V_a = new Vector2(position[0] - position1[0], position[1] - position1[1]);
        //xVelocity = (V_a.x) * 0.01f / Time.deltaTime;
        //yVelocity = (V_a.y) * 0.01f / Time.deltaTime;
        //position1 = position;
        xVelocity = Math.Abs((x_0 - x_last) * 30f * (k / 3f));
        yVelocity = Math.Abs((y_0 - y_last) * 30f * (k / 3f));
        x_last = x_0;
        y_last = y_0;

        SceneForceControl();

        SetStableForce(xForce1, xForce2, yForce1, yForce2);
        //print(xForce1);
        //print(force[0]);
        //print(xForce1 + "  " + xForce2 + "  " + yForce1 + "  " + yForce2);
        //触点-物体组合
        if ((Math.Pow(x_0 - x_1, 2) + Math.Pow(y_0 - y_1, 2)) <= C*3/k)
        {
            //物体位置
            if (!SPWholeVar.isThrdBegin)
            {
                edge = 60f / k;
                if (x_d > edge)
                {
                    x_1 = x_0 - edge;
                }
                if (x_d < -edge)
                {
                    x_1 = x_0 + edge;
                }

                if (y_d > edge)
                {
                    y_1 = y_0 - edge;
                }
                if (y_d < -edge)
                {
                    y_1 = y_0 + edge;
                }
            }
                        
            if (SPWholeVar.isFstBegin)
            {
                y_1 = y_0;
            }

            if (SPWholeVar.isThrdBegin)
            {
                if (x_d >= edge * 0.7 && y_d >= edge * 0.7)
                {
                    x_1 = x_0 - edge * 0.7f;
                    y_1 = y_0 - edge * 0.7f;
                }

                if (x_d <= -edge * 0.7 && y_d <= -edge * 0.3)
                {
                    x_1 = x_0 + edge * 0.7f;
                    y_1 = y_0 + edge * 0.3f;
                }

                if (SPWholeVar.ThrdMateralFlag == 3)
                {
                    if (x_d >= edge * 0.5 && y_d >= edge * 0.3)
                    {
                        x_1 = x_0 - edge * 0.5f;
                        y_1 = y_0 - edge * 0.3f;
                    }
                }
                else if (SPWholeVar.ThrdMateralFlag == 2)
                {
                    if (x_d >= edge * 0.5f && y_d >= edge * 0.3f)
                    {
                        x_1 = x_0 - edge * 0.5f;
                        y_1 = y_0 - edge * 0.3f;
                    }
                }
                else if (SPWholeVar.ThrdMateralFlag == 4)
                {
                    if (x_d >= edge * 0.5f && y_d >= edge * 0.3f)
                    {
                        x_1 = x_0 - edge * 0.5f;
                        y_1 = y_0 - edge * 0.3f;
                    }
                    else if (x_d <= -edge * 0.3 && y_d <= -edge * 0.3)
                    {
                        x_1 = x_0 + edge * 0.3f;
                        y_1 = y_0 + edge * 0.3f;
                    }
                }
            }
        }
        else
        {
            force[0] = 0f;
            force[1] = 0f;
        }

        if (SPWholeVar.isFstBegin)
        {
            energyForce = (float)Math.Sqrt(Math.Pow(force[0], 2) + Math.Pow(force[1], 2)) * 10;
        }
        else if (SPWholeVar.isScdBegin)
        {
            energyForce = (float)Math.Sqrt(Math.Pow(force[0], 2) + Math.Pow(force[1], 2)) * 5;
        }
        else if (SPWholeVar.isThrdBegin)
        {
            energyForce = (float)Math.Sqrt(Math.Pow(force[0], 2) + Math.Pow(force[1], 2)) * 10;
        }
        
        //得到位置信息，赋值扭力
        try
        {
            //负责位置提取和扭力赋值。
            torque = GetTorque(force);
            SetTorque(torque);
            GetDegree();
            position = Caculate.GetPosition(angles);
        }
        catch (Exception ex)
        {
            
        }

    }

    /*-----------------------Force caculation function-----------------------*/

    /**
     * 力学计算   
     */
    void SceneForceControl()
    {
        float m = 2000f;//物体质量（kg）
        float g = 9.8f;//重力系数
        float length = 2;
        float width = 1;
        float height = 1.5f;
        float kf = 300;

        float scale = 2f;
        if (SPWholeVar.isGuide)
        {

        }

        else if (SPWholeVar.isFstBegin)
        {
            float surfacePos = (1065f + 200f) / k;
            float rho = 1000f;
            float buoyancy;
            float kn = 500f;
            if ((y_0 - surfacePos) < 0)
            {
                if ((y_0 - surfacePos) > -400f / k)
                {
                    buoyancy = rho * g * (surfacePos - y_0) * height / (400f / k) * length * width;
                    yForce1 = (buoyancy - m * g) / kf / kn;
                    yForce2 = (m * g - buoyancy) / kf / kn;

                }
                else
                {
                    buoyancy = rho * g * height * length * width;
                    yForce1 = (buoyancy - m * g) / kf / kn;
                    yForce2 = (m * g - buoyancy) / kf / kn;
                }
            }
            else
            {
                yForce1 = -m * g / kf / kn;
                yForce2 = m * g / kf / kn;
            }

            if (y_1 < 1026f / k)
            {
                if ((y_1 - 1026f / k) < (-180f / k))
                {
                    waterDeep = 150;

                    xForce1 = - height * width * 0.15f;
                    xForce2 = - height * width * 0.15f;
                }
                else
                {
                    waterDeep = (1026f / k - y_1) / (180f / k) * 150f;

                    xForce1 = -(1026f / k - y_1) / (180f / k) * height * width * 0.15f;
                    xForce2 = -(1026f / k - y_1) / (180f / k) * height * width * 0.15f;
                }
            }
            else
            {
                waterDeep = 0;
            }
        }
        
        else if (SPWholeVar.isScdBegin)
        {
            float V_Wind;
            float yWindFlag;

            Vector3 wallPos = SPWholeVar.curCam.WorldToScreenPoint(SPWholeVar.WindWall.transform.position);

            yWindFlag = (Screen.width * SPWholeVar.rightEdge - x_0) /
                        (Screen.width * (SPWholeVar.rightEdge - SPWholeVar.leftEdge))
                        * (Screen.height * SPWholeVar.topEdge -
                           Screen.height * (SPWholeVar.topEdge + SPWholeVar.bottomEdge) / 2) +
                        Screen.height * (SPWholeVar.topEdge + SPWholeVar.bottomEdge) / 2;

            if (y_0 > yWindFlag)
            {
                V_Wind = 0;
            }
            else if (y_0 > (Screen.height * (SPWholeVar.topEdge + SPWholeVar.bottomEdge) / 3))
            {
                V_Wind = (yWindFlag - y_0) /
                         (yWindFlag - (Screen.height * (SPWholeVar.topEdge + SPWholeVar.bottomEdge) / 3)) * 30;
            }
            else
            {
                V_Wind = 30;
            }

            windY = V_Wind;
            float P = (float)(0.613 * Math.Pow(V_Wind / 10, 2));
            float A = length * height;
            float Cd = 1.4f;
            float F = P * A * Cd;
            xForce1 = -F / kf * scale * 2;

            xForce2 = 0;
            yForce1 = yForce2 = 0;

            if (x_0 > wallPos.x)
            {
                xForce1 = 0f;
            }

        }
        
        else if (SPWholeVar.isThrdBegin)
        {
            C = 10000;
            float muWood = 0.30f;
            float muMetal = 0.20f;
            float muRock = 0.50f;
            float muIce = 0.03f;
            float kt = 39200;
            float force0 = 0;
            edge = 60f/k;
            switch (SPWholeVar.ThrdMateralFlag)
            {
                case 1:
                    force0 = (m * g * (float)Math.Cos((float)30 / 180 * Math.PI) * muWood + m * g * (float)Math.Sin(30 / 180 * Math.PI));
                    xForce1 = -force0 * (float)Math.Cos((float)30 / 180 * Math.PI) / kt;
                    yForce1 = -force0 * (float)Math.Sin((float)30 / 180 * Math.PI) / kt;
                    xForce2 = -0.01f;
                    yForce2 = -0.01f;
                    break;
                case 2:
                    force0 = (m * g * (float)Math.Cos((float)20 / 180 * Math.PI) * muMetal + m * g * (float)Math.Sin(20 / 180 * Math.PI));
                    xForce1 = -force0 * (float)Math.Cos((float)20 / 180 * Math.PI) / kt;
                    yForce1 = -force0 * (float)Math.Sin((float)20 / 180 * Math.PI) / kt;
                    xForce2 = -0.05f;
                    yForce2 = -0.03f;
                    break;
                case 3:
                    edge = 90f / k;
                    force0 = (m * g * (float)Math.Cos((float)15 / 180 * Math.PI) * muRock + m * g * (float)Math.Sin(15 / 180 * Math.PI));
                    xForce1 = -force0 * (float)Math.Cos((float)15 / 180 * Math.PI) / kt;
                    yForce1 = -force0 * (float)Math.Sin((float)15 / 180 * Math.PI) / kt;
                    xForce2 = -0.05f;
                    yForce2 = -0.05f;
                    break;
                case 4:
                    edge = 90f / k;
                    force0 = (m * g * (float)Math.Cos((float)25 / 180 * Math.PI) * muIce + m * g * (float)Math.Sin(25f / 180 * Math.PI));
                    xForce1 = -force0 * (float)Math.Cos((float)25 / 180 * Math.PI) / kt;
                    yForce1 = -force0 * (float)Math.Sin((float)25 / 180 * Math.PI) / kt;
                    xForce2 = -0.03f;
                    yForce2 = -0.01f;
                    break;
                default:
                    xForce1 = 0.0f;
                    yForce1 = 0.0f;
                    xForce2 = 0.0f;
                    yForce2 = 0.0f;
                    break;
            }
        }
        else
        {
            xForce1 = 0;
            xForce2 = 0;
            yForce1 = 0;
            yForce2 = 0;
        }
    }


    /**
     * 稳定力反馈控制，force1,force2分别代表x正负方方向力，force3,force4分别代表x正负方方向力   
     */
    void SetStableForce(float force1, float force2, float force3, float force4)
    {

        if (!SPWholeVar.isFstBegin)
        {
            if(SPWholeVar.isScdBegin)
                edge = 60f / k;
            //x方向力反馈
            if (x_d > edge)
            {
                force[0] = -force1;
            }
            else if (x_d < -edge)
            {
                force[0] = force2;
            }
            else if (x_d >= 0 && x_d < edge)
            {
                force[0] = -Math.Abs(x_d) * force1 / edge;
            }
            else if (x_d <= 0 && x_d > -edge)
            {
                force[0] = Math.Abs(x_d) * force2 / edge;
            }
            else
                force[0] = 0;

            //y方向力反馈
            if (y_d > edge * 0.5)
            {
                force[1] = -force3;
            }
            else if (y_d < -edge * 0.5)
            {
                force[1] = force4;
            }
            else if (y_d >= 0 && y_d < edge * 0.5)
            {
                force[1] = -Math.Abs(y_d) * force3 / edge / 0.5f;
            }
            else if (y_d <= 0 && y_d > -edge * 0.5)
            {
                force[1] = Math.Abs(y_d) * force4 / edge / 0.5f;
            }
            else
            {
                force[1] = 0;
            }
        }
        else
        {
            edge = 200f / k;
            if (x_d > edge)
            {
                force[0] = -force1;
            }
            else if (x_d < -edge)
            {
                force[0] = force2;
            }
            else if (x_d >= 0 && x_d < edge)
            {
                force[0] = -Math.Abs(x_d) * force1 / edge;
            }
            else if (x_d <= 0 && x_d > -edge)
            {
                force[0] = Math.Abs(x_d) * force2 / edge;
            }
            else
                force[0] = 0;

            if (y_d > 0)
            {
                force[1] = -force3;
            }
            else
                force[1] = force4;
        }


    }




    /*-----------------------Transform function-----------------------*/

    /**
    * 初始化位置信息，offest分别为两个Ecoder的初始位置，resulation为分辨率，默认13824
    */
    void DeviceStartFunction(float offset1, float offset2)
    {
        outData[0] = command;
        DataSendFunction(0);
        SendEncoderData(offset1, resoluation, offset2, resoluation);
        DataSendFunction(activeNumber * 4 * 2);
    }

    /**
     * Send data to instialize the Encoder
     */
    void SendEncoderData(float offset1, float resulation1, float offset2, float resulation2)
    {
        int j = 1;
        byte[] segment = new byte[4];

        outData[0] = deviceAddress;
        if (activeNumber >= 1)
        {
            FloatToBytes(offset1, segment);
            ArrayCopy(segment, 0, outData, j, 4);
            j += 4;
            FloatToBytes(resulation1, segment);
            ArrayCopy(segment, 0, outData, j, 4);

            if (activeNumber == 2)
            {
                j += 4;
                FloatToBytes(offset2, segment);
                ArrayCopy(segment, 0, outData, j, 4);
                j += 4;
                FloatToBytes(resulation2, segment);
                ArrayCopy(segment, 0, outData, j, 4);
                j += 4;
            }
        }
    }


    /**
     * 读取返回的角度信息，存于receiveData中，每4位为一个角度
     */
    void DataReceiveFunction()
    {

        byte[] buffer = new byte[1 + 4 * activeNumber];
        int index = 0;//用于记录此时的数据次序  
        bool flag = true;
        int dataLength;
        if (sp != null && sp.IsOpen)
        {
            try
            {
                dataLength = sp.Read(buffer, 0, 1 + 4 * activeNumber);
                for (int i = 0; i < dataLength; i++)
                {
                    if (buffer[i] == deviceAddress && flag)
                    {
                        index = 0;//次序归0 
                        flag = false;
                        continue;
                    }
                    else
                    {
                        if (index >= receiveData.Length) index = buffer.Length - 1;
                        //理论上不应该会进入此判断，但是由于传输的误码，导致数据的丢失，使得标志位与数据个数出错  
                        receiveData[index] = buffer[i];
                        //将数据存入receiveData中  
                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }

    /**
     * 发送信息函数
     * amount:发送信息的位数
     */
    void DataSendFunction(int amount)
    {
        sp.Write(outData, 0, amount + 1);
    }

    /**
     * Send torque data
     */
    void SendTorqueData(float tro1, float tro2)
    {
        int j = 1;
        byte[] segment = new byte[4];
        outData[0] = deviceAddress;
        FloatToBytes(tro1, segment);

        if (activeNumber >= 1)
        {
            FloatToBytes(tro1, segment);
            ArrayCopy(segment, 0, outData, j, 4);

            if (activeNumber == 2)
            {
                j += 4;
                FloatToBytes(tro2, segment);
                ArrayCopy(segment, 0, outData, j, 4);
            }
        }
    }

    /**
     * Get degree of motors
     */
    void GetDegree()
    {
        int j = 0;
        byte[] segment = new byte[4];
        DataReceiveFunction();
        if (activeNumber >= 1)
        {
            ArrayCopy(receiveData, j, segment, 0, 4);
            angles[0] = BytesToFloat(segment);
            if (activeNumber == 2)
            {
                j += 4;
                ArrayCopy(receiveData, j, segment, 0, 4);
                angles[1] = BytesToFloat(segment);
            }
        }
    }

    /**
     * get torque function
     * tempForce[] 为想要施加在两个x,y两个方向的力
     * 返回torque
     */
    float[] GetTorque(float[] tempForce)
    {
        float[] temp = new float[2];
        temp = Caculate.GetTorque(tempForce);
        return temp;
    }

    /**
     * set torque function
     * tempTorque[] 为想要施加的扭矩
     */
    void SetTorque(float[] tempTorque)
    {
        float[] temp = new float[2];
        outData[0] = command1;
        DataSendFunction(0);
        SendTorqueData(tempTorque[0], tempTorque[1]);
        DataSendFunction(4 * activeNumber);
    }








    /*-----------------------Helper functions-----------------------*/


    /**
    * Translates a 32-bit floating point into an array of four bytes
    */
    void FloatToBytes(float val, byte[] segments)
    {
        uFloat temp = new uFloat();
        temp.val_f = val;
        segments[3] = (byte)((temp.val_l >> 24) & 0xff);
        segments[2] = (byte)((temp.val_l >> 16) & 0xff);
        segments[1] = (byte)((temp.val_l >> 8) & 0xff);
        segments[0] = (byte)((temp.val_l) & 0xff);
    }

    /**
     * Translates an array of four bytes into a floating point
     */
    float BytesToFloat(byte[] segments)
    {
        uFloat temp = new uFloat();
        temp.val_l = (temp.val_l | (segments[3] & 0xff)) << 8;
        temp.val_l = (temp.val_l | (segments[2] & 0xff)) << 8;
        temp.val_l = (temp.val_l | (segments[1] & 0xff)) << 8;
        temp.val_l = (temp.val_l | (segments[0] & 0xff));

        return temp.val_f;
    }

    /**
     * Copies elements from one array to another
     */
    void ArrayCopy(byte[] src, int src_index, byte[] dest, int dest_index, int len)
    {
        for (int i = 0; i < len; i++)
        {
            dest[dest_index + i] = src[src_index + i];
        }
    }

}