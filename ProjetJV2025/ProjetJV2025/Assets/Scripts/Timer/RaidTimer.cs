using UnityEngine;

public class RaidTimer : BaseTimer
{
    public override void StartTimer()
    {
        base.StartTimer();
        Debug.Log("Raid commencé !");
    }

    protected override void EndTimer()
    {
        base.EndTimer();
        Debug.Log("Raid terminé !");
    }
}
