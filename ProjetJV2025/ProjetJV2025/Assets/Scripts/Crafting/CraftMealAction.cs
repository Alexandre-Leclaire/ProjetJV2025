using System.Collections.Generic;
using UnityEngine;

public class CraftMealAction : MonoBehaviour, IActionnable
{
    public List<IActionnable.Element> GetActions()
    {
        return new List<IActionnable.Element>
        {
            new IActionnable.Element
            {
                name = "Craft Meal (10 food)",
                interactionTime = 2f,
                resetCooldown = 1f,
                cancelOnLeave = true,

                onInteract = player =>
                {
                    Inventory inv = player.GetComponent<Inventory>();
                    return inv != null && inv.Has("Food", 10);
                },

                onInteractionFinish = player =>
                {
                    Inventory inv = player.GetComponent<Inventory>();
                    inv.Remove("Food", 10);
                    inv.Add("Meal", 1);

                    Debug.Log("? Meal crafté !");
                }
            }
        };
    }
}
