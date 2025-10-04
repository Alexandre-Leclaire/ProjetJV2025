using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public void AddItem(Item.ItemType type, int qty)
    {
        Item existingItem = items.Find(i => i.type == type);

        if (existingItem != null)
        {
            existingItem.quantity += qty;
        }
        else
        {
            items.Add(new Item(type, qty));
        }

        PrintInventory();
    }

    public void RemoveItem(Item.ItemType type, int qty)
    {
        Item item = items.Find(i => i.type == type);

        if (item != null)
        {
            item.quantity -= qty;

            if (item.quantity <= 0)
            {
                items.Remove(item);
            }
        }

        PrintInventory();
    }

    public void PrintInventory()
    {
        Debug.Log("📦 Inventaire :");
        foreach (var item in items)
        {
            Debug.Log($"{item.type} x{item.quantity}");
        }
    }
}
