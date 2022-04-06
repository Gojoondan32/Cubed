using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITweener : MonoBehaviour
{
    private GameObject responseObject;
    public void OnClose(GameObject objectToTween, GameObject objectToOpen)
    {
        responseObject = objectToOpen;
        LeanTween.scale(objectToTween, new Vector3(0, 0, 0), 0.5f).setOnComplete(OnClosePair);
    }

    private void OnClosePair()
    {
        //Called after the previous object has been scaled
        if(responseObject.name == "TargetNumber")
        {
            LeanTween.scale(responseObject, new Vector3(1.5f, 1, 1), 0.5f);
        }
        else
        {
            LeanTween.scale(responseObject, new Vector3(1, 1, 1), 0.5f);
        }
        
    }
    public void OnOpen(GameObject objectToTween, GameObject objectToClose)
    {
        LeanTween.scale(objectToTween, new Vector3(1, 1, 1), 0.5f);
    }

    public void StartClosed(GameObject objectToTween)
    {
        LeanTween.scale(objectToTween, new Vector3(0, 0, 0), 0f);
    }

    public void StartOpen(GameObject objectToTween)
    {
        LeanTween.scale(objectToTween, new Vector3(1, 1, 1), 0f);
    }

    public void FadeOut(GameObject objectToTween)
    {
        LeanTween.alpha(objectToTween, 0f, 0.5f);
        Debug.Log("Fade called");
    }

}
