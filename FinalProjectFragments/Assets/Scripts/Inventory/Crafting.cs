using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    [SerializeField] Item craftedItem;
    [SerializeField] Item cristalItem;
    [SerializeField] Item ironItem;

    [SerializeField] inventory _inv;

    public void ExplosiveBoxCraft()
    {

        /*_inv.RemoveItem(cristalItem);
        _inv.RemoveItem(cristalItem);
        _inv.RemoveItem(ironItem);
        _inv.AddItem(craftedItem);*/
    }
}
