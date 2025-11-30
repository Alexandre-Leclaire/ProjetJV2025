using UnityEngine;

public class HungerSystem : MonoBehaviour
{
    public float hunger = 100f;
    public float hungerDecay = 2f;
    public float lowHungerThreshold = 20f;
    public float hpDecayWhenStarving = 5f;

    public PlayerHealth hp;

    void Update()
    {
        hunger -= hungerDecay * Time.deltaTime;
        hunger = Mathf.Clamp(hunger, 0f, 100f);

        if (hunger <= lowHungerThreshold)
        {
            hp.TakeDamage(hpDecayWhenStarving * Time.deltaTime);
        }
    }

    public void EatMeal()
    {
        hunger += 40f;
        hunger = Mathf.Clamp(hunger, 0f, 100f);
    }
}
