using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    public CraftingSystem crafting;

    public Button craftBandageButton;
    public Button craftAmmoButton;
    public Button craftMealButton;

    public KeyCode toggleKey = KeyCode.C;

    void Start()
    {
        craftBandageButton.onClick.AddListener(() => crafting.CraftBandage());
        craftAmmoButton.onClick.AddListener(() => crafting.CraftAmmo());
        craftMealButton.onClick.AddListener(() => crafting.CraftMeal());
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            gameObject.SetActive(!gameObject.activeSelf);
    }
}
