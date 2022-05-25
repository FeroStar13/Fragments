using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItem : MonoBehaviour
{
    
    [SerializeField] Item IntendedItem;


    private void OnCollisionEnter(Collision collision)
    {
        ICollectable item = collision.transform.GetComponent<ICollectable>();
        if (item == null)
        {

        }
        else
        {
            item.Collected(IntendedItem);
            Destroy(gameObject);
        }
    }
}
