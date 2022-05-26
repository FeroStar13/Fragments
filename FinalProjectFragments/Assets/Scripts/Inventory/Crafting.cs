using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    [SerializeField] List<Item> itemsNeddedToCraft;
    [SerializeField] Item craftedItem;

    [SerializeField] inventory _inv;

    [SerializeField] PlayerCharacter player;


    public void Craft1()
    {

        for (int i = 0; i < itemsNeddedToCraft.Count; i++)
        {
            if (_inv.Contains(itemsNeddedToCraft[i]) == false)
            {
                return;
            }
        }

        for (int i = 0; i < itemsNeddedToCraft.Count; i++)
        {
            _inv.RemoveItem(itemsNeddedToCraft[i]);
        }
        _inv.AddItem(craftedItem);
        player.AmmountOfExplosiveBoxes++;

        /*  _inv.RemoveItem(cristalItem);
          _inv.RemoveItem(cristalItem);
          _inv.RemoveItem(ironItem);
          _inv.AddItem(craftedItem);

          player.AmmountOfExplosiveBoxes++;*/
    }
    public void Craft2()
    {

        for (int i = 0; i < itemsNeddedToCraft.Count; i++)
        {
            if (_inv.Contains(itemsNeddedToCraft[i]) == false)
            {
                return;
            }
        }

        for (int i = 0; i < itemsNeddedToCraft.Count; i++)
        {
            _inv.RemoveItem(itemsNeddedToCraft[i]);
        }
        _inv.AddItem(craftedItem);
        player.AmmountOfEnergyCannons++;
    }

    public void Craft3()
    {
        for (int i = 0; i < itemsNeddedToCraft.Count; i++)
        {
            if (_inv.Contains(itemsNeddedToCraft[i]) == false)
            {
                return;
            }
        }

        for (int i = 0; i < itemsNeddedToCraft.Count; i++)
        {
            _inv.RemoveItem(itemsNeddedToCraft[i]);
        }
      
        player.PlayerFireCharge += 50;
        player.CanFire = true;
        UIManager.instance.UpdateCharge(player.PlayerFireCharge);
    }
}
