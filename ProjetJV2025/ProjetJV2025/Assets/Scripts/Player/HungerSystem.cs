using UnityEngine;

public class HungerSystem : MonoBehaviour
{
    public float hunger = 100f;
    public float hungerDecay = 2f;
    public float lowHungerThreshold = 20f;
    public float hpDecayWhenStarving = 5f;

    PlayerHealth hp;
    Inventory inv;

    void Awake()
    {
        hp = GetComponent<PlayerHealth>();
        inv = GetComponent<Inventory>();

        if (hp == null)
            Debug.LogError("HungerSystem : PlayerHealth manquant sur le Player");

        if (inv == null)
            Debug.LogError("HungerSystem : Inventory manquant sur le Player");
    }

    void Update()
    {
        if (hp == null)
            return;

        hunger -= hungerDecay * Time.deltaTime;
        hunger = Mathf.Clamp(hunger, 0f, 100f);

        if (hunger <= lowHungerThreshold)
        {
            hp.TakeDamage(hpDecayWhenStarving * Time.deltaTime);
        }
    }

    public void EatMeal()
    {
        if (inv == null)
            return;

        if (!inv.Has("Meal", 1))
            return;

        inv.Remove("Meal", 1);

        hunger += 40f;
        hunger = Mathf.Clamp(hunger, 0f, 100f);
    }
}
