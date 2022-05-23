using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItem : MonoBehaviour
{
    
    [SerializeField] Item IntendedItem;
    [SerializeField] inventory _inv;

    private void OnTriggerEnter(Collider other)
    {
       
        _inv.AddItem(IntendedItem);
        Destroy(gameObject);
    }
}
