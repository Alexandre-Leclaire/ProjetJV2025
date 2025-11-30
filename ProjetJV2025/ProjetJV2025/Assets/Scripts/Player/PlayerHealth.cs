using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float hp = 100f;

    public void TakeDamage(float amount)
    {
        hp -= amount;

        if (hp <= 0f)
        {
            Debug.Log("Player Dead");
        }
    }

    public void Heal(float amount)
    {
        hp = Mathf.Clamp(hp + amount, 0f, 100f);
    }
}
