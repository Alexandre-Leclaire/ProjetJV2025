using System;
using System.Collections.Generic;
using UnityEngine;

public interface IActionnable
{
    [Serializable]
    public struct Element
    {
        public string name;
        public Func<GameObject, bool> onInteract;
        public float interactionTime;
        public Action<GameObject> onInteractionFinish;
        public bool cancelOnLeave;
        public float resetCooldown;
    }

    List<Element> GetActions();
}
