using System;
using System.Collections.Generic;
using UnityEngine.AI;

public abstract class BT_Node
{
    public abstract bool Execute(NavMeshAgent agent);
}

public class BT_Action : BT_Node
{
    private Func<bool, NavMeshAgent> action;
    public BT_Action(Func<bool, NavMeshAgent> action)
    {
        this.action = action;
    }

    public override bool Execute(NavMeshAgent agent)
    {
        return action(agent);
    }
}

public class BT_Selector : BT_Node
{
    private List<Func<bool, NavMeshAgent>> actions;

    public BT_Selector(List<Func<bool, NavMeshAgent>> actions)
    {
        this.actions = actions;
    }

    public override bool Execute(NavMeshAgent agent)
    {
        throw new NotImplementedException();
    }
}

public class BT_Sequence : BT_Node
{
    private List<Func<bool, NavMeshAgent>> actions;

    public BT_Sequence(List<Func<bool, NavMeshAgent>> actions)
    {
        this.actions = actions;
    }

    public override bool Execute(NavMeshAgent agent)
    {
        throw new NotImplementedException();
    }
}
