using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new();

    public void Add(string id, int qty)
    {
        Item item = items.Find(i => i.id == id);

        if (item != null)
            item.quantity += qty;
        else
            items.Add(new Item(id, qty));
    }

    public bool Has(string id, int qty)
    {
        Item item = items.Find(i => i.id == id);
        return item != null && item.quantity >= qty;
    }

    public bool Remove(string id, int qty)
    {
        Item item = items.Find(i => i.id == id);
        if (item == null || item.quantity < qty)
            return false;

        item.quantity -= qty;

        if (item.quantity <= 0)
            items.Remove(item);

        return true;
    }
}
