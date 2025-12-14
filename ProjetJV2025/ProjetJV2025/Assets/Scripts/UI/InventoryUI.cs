using TMPro;
using UnityEngine;
using System.Text;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;
    TMP_Text inventoryText;

    void Awake()
    {
        inventoryText = GetComponent<TMP_Text>();
        if (inventoryText == null)
        {
            Debug.LogError("[InventoryUI] No TMP_Text found on this GameObject");
        }

        inventory = FindFirstObjectByType<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("[InventoryUI] No Inventory found in scene");
        }
    }

    void Update()
    {
        if (inventory == null || inventoryText == null)
            return;

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Resources");
        foreach (ResourceType r in System.Enum.GetValues(typeof(ResourceType)))
        {
            sb.AppendLine($"{r}: {inventory.GetQuantity(r.ToString())}");
        }

        sb.AppendLine();
        sb.AppendLine("Crafted");
        foreach (CraftedItemType c in System.Enum.GetValues(typeof(CraftedItemType)))
        {
            sb.AppendLine($"{c}: {inventory.GetQuantity(c.ToString())}");
        }

        inventoryText.text = sb.ToString();
    }
}
