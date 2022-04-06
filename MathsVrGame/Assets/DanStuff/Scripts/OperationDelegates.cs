using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationDelegates : MonoBehaviour
{
    #region Singleton
    public static OperationDelegates instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion
    //public delegate int Add(int a, int b);
    //private Add add;

    public Func<int, int, int> Add = (a, b) => a + b;
    public Func<int, int, int> Subtract = (a, b) => a - b;
    public Func<int, int, int> Multiply = (a, b) => a * b;
    public Func<int, int, int> Divide = (a, b) => a / b;
    public Func<float, float, float> Exponent = (a, b) => Mathf.Pow(a, b);



}
