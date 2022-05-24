using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour
{
    [SerializeField] protected List<Item> items;
    [SerializeField] Transform itemsParant;
    [SerializeField] protected ItemSlot[] itemSlots;

    private void OnValidate()
    {
        if (itemsParant != null)
        {
            itemSlots = itemsParant.GetComponentsInChildren<ItemSlot>();
        }
        RefreshUI();
    }

    void RefreshUI()
    {
        int i = 0;

        for (; i < items.Count && i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = items[i];
        }

        for (; i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = null;
        }
    }

    public bool AddItem(Item item)
    {
        if (IsFull())
            return false;

        items.Add(item);
        RefreshUI();
        return true;
    }

    public bool RemoveItem(Item item)
    {
        if (items.Remove(item))
        {
            RefreshUI();
            return true;
        }
        return false;
    }

    public bool IsFull()
    {
        return items.Count >= itemSlots.Length;
    }

    public bool Contains(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if(item == items[i])
            {
                return true;
            }
        }
        return false;
    }
}
