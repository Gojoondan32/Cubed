using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSpawner : MonoBehaviour
{
    ObjectPooler objectPooler;
    Vector3 offest;
    private float startTime = 0.3f;
    [SerializeField] private Transform pivotPoint;
    private Vector3 previousPosition;


    private void Start()
    {
        objectPooler = ObjectPooler.instance;
        
    }

    private void FixedUpdate()
    {
        startTime -= Time.deltaTime;
        if(startTime <= 0)
        {
            SpawnCube();
            startTime = 0.3f;
        }
    }

    private Vector3 CreateCircle(Vector3 centre, float length)
    {
        Vector3 position;
        //Get a random value between 0 and 360
        float value = Random.value * 360;

        //Use trigononmatry to create a perfect circle around a given point
        position.x = centre.x + length * Mathf.Cos(value * Mathf.Deg2Rad);
        position.y = transform.position.y;
        position.z = centre.z + length * Mathf.Sin(value * Mathf.Deg2Rad);
        

        if (AvailableArea(position))
        {
            previousPosition = position;
            return position;
        }

        return Vector3.zero;
    }

    private void SpawnCube()
    {
        //Define the centre point of the circle
        Vector3 centre = transform.position;
        for (int i = 0; i < 1; i++)
        {
            Vector3 position = CreateCircle(centre, 0.7f);
            if(position != Vector3.zero)
            {
                //Rotate the cubes to look at the given point
                Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, centre - position);

                if(GameState.instance.CurrentState == GameState.State.Game)
                {
                    objectPooler.SpawnFromPool("NumberCube", position, rotation);
                }
                else if (GameState.instance.CurrentState == GameState.State.Menu || GameState.instance.CurrentState == GameState.State.Help)
                {
                    objectPooler.SpawnFromPool("MenuBlock", position, rotation);
                }
            }

        }
    }

    private bool AvailableArea(Vector3 position)
    {
        //Fire a raycast down from the position to see if it hits the table 
        LayerMask intersectionMask = LayerMask.GetMask("Table");
        if (Physics.Raycast(position, Vector3.down, Mathf.Infinity, intersectionMask) || (previousPosition - position).magnitude < 0.5f)
        {
            return false;

        }
        return true;

    }
}
