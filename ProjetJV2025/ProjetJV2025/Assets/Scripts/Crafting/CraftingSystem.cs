using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public Inventory inv;

    public bool CraftBandage()
    {
        if (Has(Item.ItemType.Cloth, 25))
        {
            inv.RemoveItem(Item.ItemType.Cloth, 25);
            inv.AddItem(Item.ItemType.Bandage, 1);
            return true;
        }
        return false;
    }

    public bool CraftAmmo()
    {
        if (Has(Item.ItemType.Metal, 15))
        {
            inv.RemoveItem(Item.ItemType.Metal, 15);
            inv.AddItem(Item.ItemType.Ammo, 5);
            return true;
        }
        return false;
    }

    public bool CraftMeal()
    {
        if (Has(Item.ItemType.Food, 20))
        {
            inv.RemoveItem(Item.ItemType.Food, 20);
            inv.AddItem(Item.ItemType.Meal, 1);
            return true;
        }
        return false;
    }

    bool Has(Item.ItemType type, int qty)
    {
        Item it = inv.items.Find(i => i.type == type);
        return it != null && it.quantity >= qty;
    }
}
