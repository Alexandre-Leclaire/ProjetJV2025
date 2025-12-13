[System.Serializable]
public class Item
{
    public string id;
    public int quantity;

    public Item(string id, int qty)
    {
        this.id = id;
        quantity = qty;
    }
}
