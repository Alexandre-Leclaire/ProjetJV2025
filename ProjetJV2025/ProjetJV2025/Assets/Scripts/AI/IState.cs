using UnityEngine.AI;

public interface IState 
{
    IAiEntity Entity { get; }
    void Execute();
}
