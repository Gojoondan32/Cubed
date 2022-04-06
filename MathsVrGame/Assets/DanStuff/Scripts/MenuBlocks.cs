using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBlocks : Blocks
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            ObjectPooler.instance.QueueObjects(this.gameObject, "MenuBlock");
        }
    }
}
