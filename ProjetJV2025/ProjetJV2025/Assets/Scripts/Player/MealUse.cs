using StarterAssets;
using UnityEngine;

public class MealUse : MonoBehaviour
{
    Inventory inv;
    ThirdPersonController controller;

    void Awake()
    {
        inv = GetComponent<Inventory>();
        controller = GetComponent<ThirdPersonController>();

        if (inv == null)
            Debug.LogError("MealUse : Inventory manquant sur le Player");

        if (controller == null)
            Debug.LogError("MealUse : ThirdPersonController manquant sur le Player");
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.M))
            return;

        if (inv == null || controller == null)
            return;

        // ❌ déjà sous effet → on bloque
        if (controller.staminaProtected)
        {
            Debug.Log("MealUse : stamina already protected");
            return;
        }

        if (!inv.Has("Meal", 1))
            return;

        inv.Remove("Meal", 1);
        controller.EnableStaminaProtection();
    }
}
