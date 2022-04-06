using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class BlockPlacer : MonoBehaviour
{
    [SerializeField] private LinearMapping linearMapping;

    ObjectPooler objectPooler;


    [SerializeField] private GameObject prefab;
    public static bool inMenu;
    private bool spawnMenuPlacers = true;

    private float total = 10;
    private int counter;

    private void Start()
    {
        spawnMenuPlacers = true;
        counter = UpdateCounter();
        objectPooler = ObjectPooler.instance;
    }
    private void Update()
    {

        //Update the blocks length if the player has modifed the slider
        if (counter != UpdateCounter())
        {
            UpdateListLength();
            Debug.Log("Updating list");
        }


    }

    public void UpdateListLength(){
        objectPooler.DisablePool("BlockPlacer");
        counter = UpdateCounter();
        

        if(GameState.instance.CurrentState == GameState.State.Game)
        {
            //Update list to represent the linearMapping count 
            for (int i = 0; i < UpdateCounter(); i++)
            {
                GameObject placer = objectPooler.SpawnFromPool("BlockPlacer", UpdatePosition(i), Quaternion.identity);

                //Update the position of the placer
                PlacerBase placerPosition = placer.GetComponent<PlacerBase>();
                placerPosition.Index = i;
            }
        }
        else if(GameState.instance.CurrentState == GameState.State.Menu && spawnMenuPlacers)
        {
            GameObject menuPlacer = objectPooler.SpawnFromPool("MenuPlacer", UpdatePosition(2), Quaternion.identity);
            spawnMenuPlacers = false;
        }

    }



    private int UpdateCounter(){
        //Multiply the linearMapping value by 10 so that it can be used to determine the length of the list
        float converter = linearMapping.value * 10;

        //Convert the linearMapping value to an int 
        int count = (int)Mathf.Round(converter);
        return count;
    }

    private Vector3 UpdatePosition(int index)
    {
        Vector3 start = gameObject.transform.GetChild(0).position;
        Vector3 end = gameObject.transform.GetChild(1).position;

        //Get the percentage value we need for the spaces between the blocks 
        //Divide by 10 to get a decimal value
        float percentage = (total / (UpdateCounter() + 1) / 10);
        
        //Use the percentage previously to calculate where that percentage would be on the magnitude 
        float magnitude = (end - start).magnitude;
        magnitude *= percentage;

        //Define the starting position
        float pos = magnitude * index;
        pos += magnitude + start.x;

        return new Vector3(pos, transform.position.y, transform.position.z);
    }


    
}
