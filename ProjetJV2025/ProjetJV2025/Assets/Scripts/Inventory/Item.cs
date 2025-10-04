[System.Serializable]
public class Item
{
    public string itemName;
    public int quantity;

    public Item(string name, int qty)
    {
        itemName = name;
        quantity = qty;
    }
}
