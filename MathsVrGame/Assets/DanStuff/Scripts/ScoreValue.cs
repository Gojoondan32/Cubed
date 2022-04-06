using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreValue : MonoBehaviour
{
    #region Singleton
    public static ScoreValue instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion


    private int score;
    public int Score { get { return score; } }

    public static int BlockAmount = 0;

    public void UpdateScore(int points)
    {
        //Take into account the amount of blocks and the target value they were aiming for
        //CURRENTLY DOES NOT WORK FIX THIS 

        Debug.Log("amount of blocks " + BlockAmount.ToString());
        Debug.Log("previous value " + TargetValue.instance.Result);
        //Determine the amount of blocks used 


        //This is the old function
        //points -= TargetValue.instance.Result / BlockAmount;

        //This is the new function
        score += (TargetValue.instance.Result / BlockAmount) + points;


        Debug.Log("Amount of points earned: " + points.ToString());

    }
}
