using System.Collections.Generic;
using UnityEngine;

public class CraftingActionnable : MonoBehaviour, IActionnable
{
    public List<IActionnable.Element> GetActions()
    {
        return new List<IActionnable.Element>
        {
            Craft(
                "Craft Bandage",
                2f,
                "Cloth", 25,
                "Bandage", 1
            ),
            Craft(
                "Craft Ammo",
                3f,
                "Metal", 15,
                "Ammo", 5
            ),
            Craft(
                "Craft Meal",
                4f,
                "Food", 20,
                "Meal", 1
            )
        };
    }

    IActionnable.Element Craft(
        string name,
        float time,
        string inputId,
        int inputQty,
        string outputId,
        int outputQty
    )
    {
        return new IActionnable.Element
        {
            name = name,
            interactionTime = time,
            resetCooldown = 1f,
            cancelOnLeave = true,

            onInteract = player =>
            {
                Inventory inv = player.GetComponent<Inventory>();
                return inv != null && inv.Has(inputId, inputQty);
            },

            onInteractionFinish = player =>
            {
                Inventory inv = player.GetComponent<Inventory>();
                inv.Remove(inputId, inputQty);
                inv.Add(outputId, outputQty);
            }
        };
    }
}
