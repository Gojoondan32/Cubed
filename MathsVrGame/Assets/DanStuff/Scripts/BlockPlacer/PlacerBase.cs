using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacerBase : MonoBehaviour
{
    //Hold a reference for the position of the block placer
    private int index;
    private int currentValue;

    public int Index { get { return index; } set { index = value; } }

    public int CurrentValue { get { return currentValue; } set { currentValue = value; } }


}

