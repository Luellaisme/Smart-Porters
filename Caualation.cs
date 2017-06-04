using UnityEngine;
using System.Collections;
using System;

public class Caualation
{

    /*Caculation funcitong*/

    private float l = 0.05f;
    private float L = 0.065f;
    private float d = 0.02f;


    private float th1, th2;
    private float tau1, tau2;
    private float f_x, f_y;
    private float x_E, y_E;

    private float J11, J12, J21, J22; //Jacobian 
    private float gain = 0.1f;

    public void forwardKinematics(float[] angles)
    {
        th1 = (float)Math.PI / 180 * angles[0];
        th2 = (float)Math.PI / 180 * angles[1];

        float c1 = (float)Math.Cos(th1);
        float c2 = (float)Math.Cos(th2);
        float s1 = (float)Math.Sin(th1);
        float s2 = (float)Math.Sin(th2);

        float xA = l * c1;
        float yA = l * s1;
        float xB = d + l * c2;
        float yB = l * s2;
        float R = (float)Math.Pow(xA, 2) + (float)Math.Pow(yA, 2);
        float S = (float)Math.Pow(xB, 2) + (float)Math.Pow(yB, 2);
        float M = (yA - yB) / (xB - xA);
        float N = (float)0.5 * (S - R) / (xB - xA);
        float a = (float)Math.Pow(M, 2) + 1;
        float b = 2 * (M * N - M * xA - yA);
        float c = (float)Math.Pow(N, 2) - 2 * N * xA + R - (float)Math.Pow(L, 2);
        float Delta = (float)Math.Pow(b, 2) - 4 * a * c;

        //float y1 = (-b + (float)Math.Sqrt(Delta)) / (2 * a);
        //float x1 = M * y1 + N;

        y_E = (-b + (float)Math.Sqrt(Delta)) / (2 * a);
        x_E = M * y_E + N;

        //if (M * y1 + N - x1 < 0)
        //{
        //    y_E = (-b - (float)Math.Sqrt(Delta)) / (2 * a);
        //    x_E = M * y_E + N;
        //}
        //else
        //{
        //    y_E = (-b + (float)Math.Sqrt(Delta)) / (2 * a);
        //    x_E = M * y_E + N;
        //}

        float phi1 = (float)Math.Acos((x_E - l * c1) / L);
        float phi2 = (float)Math.Acos((x_E - d - l * c2) / L);
        float s21 = (float)Math.Sin(phi2 - phi1);
        float s12 = (float)Math.Sin(th1 - phi2);
        float s22 = (float)Math.Sin(th2 - phi2);
        J11 = -(s1 * s21 + (float)Math.Sin(phi1) * s12) / s21;
        J12 = (c1 * s21 + (float)Math.Cos(phi1) * s12) / s21;
        J21 = (float)Math.Sin(phi1) * s22 / s21;
        J22 = -(float)Math.Cos(phi1) * s22 / s21;
    }

    public void torqueCalculation(float[] force)
    {
        f_x = force[0];
        f_y = force[1];

        tau1 = J11 * f_x + J12 * f_y;
        tau2 = J21 * f_x + J22 * f_y;

        tau1 = -tau1 * gain;
        tau2 = tau2 * gain;
    }

    public float[] GetPosition(float[] angles)
    {        
        forwardKinematics(angles);
        float[] position = { this.x_E, this.y_E };
        return position;
    }

    public float[] GetTorque(float[] force)
    {
        torqueCalculation(force);
        float[] torque = { tau1, tau2 };
        return torque;
    }
}
