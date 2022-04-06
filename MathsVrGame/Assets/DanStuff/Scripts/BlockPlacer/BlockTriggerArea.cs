using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EquationHub;


public class BlockTriggerArea : MonoBehaviour
{
    [SerializeField] private GameObject currentBlock;
    public GameObject CurrentBlock { get { return currentBlock; } set { currentBlock = value; } }

    private float timeCheck = 1f;
    public virtual void OnTriggerEnter(Collider other)
    {
        if(currentBlock == null && other.gameObject.CompareTag("Block"))
        {
            other.gameObject.transform.position = this.transform.position;
            other.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            currentBlock = other.gameObject;
            ScoreValue.BlockAmount += 1;
        }



    }

    public virtual void OnTriggerStay(Collider other)
    {
        //&& other.gameObject == currentBlock

        Blocks blocks = other.gameObject.GetComponent<Blocks>();
        if (other.gameObject == CurrentBlock && !blocks.inHand )
        {
            
            PlacerBase placerBase = gameObject.GetComponentInParent<PlacerBase>();
            
            Debug.Log("Rotating");
            //Rotate the object on the spot
            other.gameObject.transform.RotateAround(transform.position, Vector3.up, 1f);
            //other.gameObject.transform.position = this.gameObject.transform.position;

            blocks.onPlacer = true;

            placerBase.CurrentValue = blocks.value;
            EquationManager.instance.DetermineType(placerBase.Index, placerBase.CurrentValue);
            
        }


    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Block") && other.gameObject == currentBlock)
        {
            Blocks blocks = other.gameObject.GetComponent<Blocks>();
            PlacerBase placerBase = gameObject.GetComponentInParent<PlacerBase>();

            blocks.onPlacer = false;

            placerBase.CurrentValue = 0;
            //Remove the blocks value from the list
            EquationManager.instance.DetermineType(placerBase.Index);

            currentBlock = null;
            ScoreValue.BlockAmount -= 1;
        }

    }

    public virtual void RemoveBlocksOnPlacer()
    {
        if(currentBlock != null)
        {
            currentBlock.GetComponent<BoxCollider>().isTrigger = true;
            //Disable the graphics so it looks like the block disappears 
            currentBlock.GetComponentInChildren<MeshRenderer>().enabled = false;
            currentBlock.GetComponent<Rigidbody>().useGravity = false;

            //Re-enable force to push  the blocks down
            currentBlock.GetComponent<Blocks>().hasBeenGrabbed = false;
        }
    }


}
