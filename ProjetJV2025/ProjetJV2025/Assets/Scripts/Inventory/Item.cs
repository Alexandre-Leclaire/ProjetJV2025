using UnityEngine;

[System.Serializable]
public class Item
{
    public enum ItemType
    {
        Food,
        Cloth,
        Metal,
        Bandage,
        Ammo,
        Meal
    }

    public ItemType type;
    public int quantity;

    public Item(ItemType type, int qty)
    {
        this.type = type;
        this.quantity = qty;
    }
}
