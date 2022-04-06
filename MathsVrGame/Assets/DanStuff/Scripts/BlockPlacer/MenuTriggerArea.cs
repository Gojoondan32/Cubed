using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTriggerArea : BlockTriggerArea
{
    public override void OnTriggerEnter(Collider other)
    {
        if (CurrentBlock == null && other.gameObject.CompareTag("Block"))
        {
            other.gameObject.transform.position = this.transform.position;
            other.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            CurrentBlock = other.gameObject;

        }
    }
    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }
    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
