using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public void AddItem(string name, int qty)
    {
        Item existingItem = items.Find(i => i.itemName == name);

        if (existingItem != null)
        {
            existingItem.quantity += qty;
        }
        else
        {
            items.Add(new Item(name, qty));
        }

        PrintInventory();
    }

    public void RemoveItem(string name, int qty)
    {
        Item item = items.Find(i => i.itemName == name);

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
            Debug.Log($"{item.itemName} x{item.quantity}");
        }
    }
}
