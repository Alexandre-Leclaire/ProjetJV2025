using UnityEngine;
using UnityEngine.AI;

public interface IAiEntity
{
    NavMeshAgent Agent { get; }
    IState State { get; set; }
}
