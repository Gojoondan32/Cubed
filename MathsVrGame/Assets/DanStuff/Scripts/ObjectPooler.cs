using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EquationHub;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject[] prefab;
        public int size;
    }

    #region Singleton
    public static ObjectPooler instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    // Start is called before the first frame update
    void Start()
    {
        //Create an instance of the dictionary 
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            //Create a queue to add the objects
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++)
            {
                //Spawn a random object from the array
                GameObject obj = Instantiate(pool.prefab[Random.Range(0, pool.prefab.Length)]);
                obj.SetActive(false);
                //Add all objects to the queue
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    //Call this function to spawn a prefab
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Tag doesn't exist");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        if(objectToSpawn.transform.childCount > 0)
        {
            objectToSpawn.GetComponentInChildren<MeshRenderer>().enabled = true;
        }

        //Exclude blocks from being re-added to the queue straight away
        if(tag != "NumberCube" && tag != "MenuBlock")
        {
            poolDictionary[tag].Enqueue(objectToSpawn);
        }

        return objectToSpawn;
    }

    
    public void QueueObjects(GameObject obj, string tag)
    {
        obj.SetActive(false);
        //Re-add the blocks back to the queue
        poolDictionary[tag].Enqueue(obj);
    }
   

    //Used in blockPlacer
    //Disable all placers before more can be spawned to keep positions 
    public void DisablePool(string tag){
        foreach (GameObject placer in poolDictionary[tag])
        {
            //Remove values from equation manager
            int index = placer.GetComponent<PlacerBase>().Index;
            EquationManager.instance.RemoveValues(index);

            //Set the current block of each placer to null
            BlockTriggerArea triggerArea = placer.GetComponentInChildren<BlockTriggerArea>();
            triggerArea.CurrentBlock = null;

            placer.SetActive(false);
        }
    }
}
