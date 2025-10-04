[System.Serializable]
public class Item
{
    public enum ItemType
    {
        Bois,
        Pierre,
        Métal,
        Nourriture,
        Or,
        Potion
    }

    public ItemType type;
    public int quantity;

    public Item(ItemType itemType, int qty)
    {
        type = itemType;
        quantity = qty;
    }
}
