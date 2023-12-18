using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics; 
public class GraphingCalculator : MonoBehaviour
{
    [SerializeField] private GameObject pixelPrefab;
    [SerializeField] private float quality;
    [SerializeField] private float domain;
    [SerializeField] private GameObject TangentPlane;
    [SerializeField] private float derivative;
    [SerializeField] private int limit;
    [SerializeField] private int speed;
    [SerializeField] private int animationBound;
    private float deltaX;
    public float w;
    float resolution;
    bool animationSign;

    void FixedUpdate()
    {
        resolution = quality*domain;
        foreach (var obj in GameObject.FindGameObjectsWithTag("pixel"))
        {
            Destroy(obj);
        }
        RenderFrame();
        RenderTangent(derivative);
        if(w< -animationBound) {
            animationSign=true;
        }
        if (w > animationBound) {
            animationSign=false;
        }
        if(animationSign){
            w += (float)speed/100;
        }else{
            w -= (float)speed/100;
        }

    }

    void RenderTangent(float point)
    { 
        Complex output = evaluate(new Complex(point,w));
        Complex slope = estimateSlope(point);
        TangentPlane.transform.position = new UnityEngine.Vector3(point, (float)output.Real, (float)output.Imaginary);
        TangentPlane.transform.eulerAngles = new UnityEngine.Vector3(-(float)slope.Imaginary,0, (float)slope.Real);
    }

    void RenderFrame()
    {
        deltaX = domain/resolution;
        for (int i = -Mathf.RoundToInt(resolution); i < Mathf.RoundToInt(resolution); i++)
        {
            Complex output = evaluate(new Complex( i*deltaX,w));
            UnityEngine.Vector3 coords = new UnityEngine.Vector3(i * deltaX, (float)output.Real, (float)output.Imaginary);
            
            Instantiate(pixelPrefab,coords, new UnityEngine.Quaternion());
        }
    }
    Complex estimateSlope(Complex input)
    {
        float h = 1/(float)Math.Pow(10, limit);
        Complex offsetInput = Complex.Add(input, new Complex(h, h));
        Complex rise = Complex.Subtract(evaluate(offsetInput), evaluate(input));
        Complex finalAngle = new Complex(Math.Atan(Complex.Divide(rise, new Complex(h,h)).Real)*180/Math.PI,Math.Atan(Complex.Divide(rise, h).Imaginary)*180/Math.PI);
        // Complex finalAngle = new Complex(Math.Atan(Complex.Cos(input).Real)*180/Math.PI,Math.Atan(Complex.Cos(input).Imaginary)*180/Math.PI);
        // Debug.Log(Complex.Divide(rise, new Complex(h,h)).Real);
        return finalAngle;
        // return Complex.Cos(input);
    }
    Complex evaluate(Complex input)
    {
        // return Complex.Add(Complex.Pow(input, new Complex(2, 0)),Complex.One);
        Complex output = new Complex();
        // output = Complex.Pow(new Complex(Math.E, 0), input);
        // output = Complex.Add(Complex.Subtract(Complex.Add(Complex.Subtract(Complex.Multiply(Complex.Pow(input, 3),new Complex(3,0)),Complex.Pow(input,2)),Complex.One),Complex.Pow(input,4)),new Complex(5,0));
        // output = Complex.Add(Complex.Pow(input, new Complex(2,0)),7);
        // output = Complex.Log(input);
        //output=Complex.Log(input,input);
        // output = Complex.Multiply(input, Complex.ImaginaryOne);
        // output = Complex.Sin(input);
        output = Complex.Sin(Complex.Pow(input,2));
        // output=Complex.Pow(input,Complex.Cos(input));
        // return new Complex(0,0);
        // output = Complex.Pow(new Complex(Math.E,0),input);
        // output = Complex.Pow(input,input);
        // output=Complex.Sqrt(input);
        // output=input;
        // output=Complex.Sqrt(Complex.One-Complex.Pow(input,new Complex(2,0)));
        return output;
    }
    
}
