using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HandleFlash : MonoBehaviour
{
    #region Singleton
    public static HandleFlash instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializeField] private Material handleMat;
    [SerializeField] private LinearMapping linearMapping;
    [SerializeField] float countTimer = 0.2f;
    public static bool hasChangedValue = false;

    private int count = 0;
    private float linearValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Remove invoke repeating

        linearValue = linearMapping.value;

        //handleMat.EnableKeyword("_EMISSION");
    }

    // Update is called once per frame
    void Update()
    {
        countTimer -= Time.deltaTime;
        //Linear mapping used here to see if the slider has not been touched by the player
        if(countTimer <= 0f && linearValue == linearMapping.value)
        {
            StartFlash();
            countTimer = 0.2f;
        }
    }


    private void StartFlash()
    {
        switch (count)
        {
            //Change an integer value each time the function is called
            case 0:
                handleMat.color = Color.cyan;
                count++;
                break;
            case 1:
                handleMat.color = Color.red;
                count--;
                break;
        }
    }
}
