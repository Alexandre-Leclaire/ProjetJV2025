using UnityEngine;

public class BandageUse : MonoBehaviour
{
    public Inventory inv;
    public PlayerHealth hp;
    public float healAmount = 30f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Item bandage = inv.items.Find(i => i.type == Item.ItemType.Bandage);

            if (bandage != null && bandage.quantity > 0)
            {
                inv.RemoveItem(Item.ItemType.Bandage, 1);
                hp.Heal(healAmount);
            }
        }
    }
}
