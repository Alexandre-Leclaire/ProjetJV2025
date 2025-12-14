using System.Collections.Generic;
using UnityEngine;

public class CraftBandageAction : MonoBehaviour, IActionnable
{
    public List<IActionnable.Element> GetActions()
    {
        return new List<IActionnable.Element>
        {
            new IActionnable.Element
            {
                name = "Craft Bandage (15 cloth)",
                interactionTime = 2f,
                resetCooldown = 1f,
                cancelOnLeave = true,

                onInteract = player =>
                {
                    Inventory inv = player.GetComponent<Inventory>();
                    return inv != null && inv.Has("Cloth", 15);
                },

                onInteractionFinish = player =>
                {
                    Inventory inv = player.GetComponent<Inventory>();
                    inv.Remove("Cloth", 15);
                    inv.Add("Bandage", 1);

                    Debug.Log("? Bandage crafté !");
                }
            }
        };
    }
}
