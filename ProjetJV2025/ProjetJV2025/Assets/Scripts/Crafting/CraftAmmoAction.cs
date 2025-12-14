using System.Collections.Generic;
using UnityEngine;

public class CraftAmmoAction : MonoBehaviour, IActionnable
{
    public List<IActionnable.Element> GetActions()
    {
        return new List<IActionnable.Element>
        {
            new IActionnable.Element
            {
                name = "Craft Ammo (20 metal for 30 ammos)",
                interactionTime = 2f,
                resetCooldown = 1f,
                cancelOnLeave = true,

                onInteract = player =>
                {
                    Inventory inv = player.GetComponent<Inventory>();
                    return inv != null && inv.Has("Metal", 20);
                },

                onInteractionFinish = player =>
                {
                    Inventory inv = player.GetComponent<Inventory>();
                    inv.Remove("Metal", 20);
                    inv.Add("Ammo", 30);

                    Debug.Log("? Ammo crafté !");
                }
            }
        };
    }
}
