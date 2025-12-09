using UnityEngine;

public class MissionTimer : BaseTimer
{
    public string missionName = "Mission sans nom";

    public override void StartTimer()
    {
        base.StartTimer();
        Debug.Log($"Mission '{missionName}' lancée !");
    }

    protected override void EndTimer()
    {
        base.EndTimer();
        Debug.Log($"Mission '{missionName}' réussie (ou terminée)");
    }
}
