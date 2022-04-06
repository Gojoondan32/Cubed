using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem.Sample;

[RequireComponent(typeof(Rigidbody))]
public class Blocks : MonoBehaviour
{
    private Transform returnPoint;
    
    Rigidbody rb;
    [SerializeField] private float force = 3f;
    private float timeValue = 5f;

    private float timeCooldown = 5f;

    public int value = 0;
    [SerializeField] private string type;
    public bool onPlacer = false;
    public bool inHand = false;

    public bool hasBeenGrabbed = false;
    /*
    ---------------------------Notes-----------------------------------------
    Make blocks fall uniformly - currently they push each other out of the radius
    Disable freezing constraints on block's rigidbody's rotation
    Remember to subscribe to throwable script events (On pickup enable gravity or something)
    Remember to subscribe to interactable hover events (Change colour or something)
    After the player has thrown a block, smoothly move it back to the 
    */

    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.GetComponent<BoxCollider>().isTrigger = true;

        //Disable gravity
        rb.useGravity = false;

        if(gameObject.GetComponent<LockToPoint>() != null)
        {
            gameObject.GetComponent<LockToPoint>().enabled = false;
        }
        GameObject blockTransform = ObjectPooler.instance.SpawnFromPool("BlockTransform", this.gameObject.transform.position, Quaternion.identity);
        returnPoint = blockTransform.transform;
        //SetRadius(numberSpawnerLocation.transform.position, 3f);
        
    }

    public virtual void FixedUpdate()
    {
        if (!hasBeenGrabbed)
        {
            ApplyManualGravity();
        }
        TrackPosition();

        //Stop blocks from getting stuck at the top 
        timeValue -= Time.deltaTime;
        if(timeValue <= 0f)
        {
            timeValue = 5f;
            if(transform.position.y >= 2.5f)
            {
                Debug.Log("Block is stuck");
                //StartCoroutine(PreventStatic());
            }
        }
    }

    private IEnumerator PreventStatic()
    {
        hasBeenGrabbed = true;
        yield return new WaitForSeconds(0.2f);
        hasBeenGrabbed = false;
    }

    private void ApplyManualGravity(){
        //rb.AddForce(Vector3.down * force * Time.deltaTime);
        //rb.MovePosition(Vector3.down * force * Time.deltaTime);
        //rb.useGravity = false;
        Debug.Log("Block is falling");
        rb.velocity = Vector3.down * force * Time.deltaTime;
    }

    public void AttachedToHandTest()
    {
        Debug.Log("Attached to hand");
        inHand = true;
        gameObject.GetComponent<BoxCollider>().isTrigger = false;
        hasBeenGrabbed = true;
        rb.useGravity = true;
    }

    public void DetatchedFromHand()
    {
        inHand = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            this.gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        hasBeenGrabbed = false;
        if(other.gameObject.CompareTag("Ground"))
        {
            //Add the block to the queue
            ObjectPooler.instance.QueueObjects(this.gameObject, "NumberCube");
        }
    }

    private void TrackPosition()
    {
        GameObject numberSpawnerLocation = GameObject.FindGameObjectWithTag("NumberSpawner");

        //Ignore the Y value when calculating the distance
        float trueDistancePos = (numberSpawnerLocation.transform.position.x - gameObject.transform.position.x) + (numberSpawnerLocation.transform.position.z - gameObject.transform.position.z);
        
        LockToPoint lockToPoint = gameObject.GetComponent<LockToPoint>();

        if (trueDistancePos > 1.4f || trueDistancePos < -2.4f)
        {
            Debug.Log("return " + returnPoint.ToString());
            //Block is outside of the radius 
            lockToPoint.enabled = true;

            lockToPoint.snapTo = returnPoint;

            Debug.Log("Outside radius " + trueDistancePos.ToString());
        }
        else if((returnPoint.position - transform.position).magnitude < 0.1f)
        {
            Debug.Log("Close to transform");
            lockToPoint.enabled = false;
            hasBeenGrabbed = false;
            rb.useGravity = false;
        }
        

    }

}
