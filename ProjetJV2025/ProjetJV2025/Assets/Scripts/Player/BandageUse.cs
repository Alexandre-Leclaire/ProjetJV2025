using UnityEngine;

public class BandageUse : MonoBehaviour
{
    Inventory inv;
    PlayerHealth hp;
    public float healAmount = 30f;

    void Awake()
    {
        inv = GetComponent<Inventory>();
        hp = GetComponent<PlayerHealth>();

        if (inv == null)
            Debug.LogError("BandageUse : Inventory manquant sur le Player");

        if (hp == null)
            Debug.LogError("BandageUse : PlayerHealth manquant sur le Player");
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.H))
            return;

        if (inv == null || hp == null)
            return;

        if (!inv.Has("Bandage", 1))
            return;

        inv.Remove("Bandage", 1);
        hp.Heal(healAmount);
    }
}
