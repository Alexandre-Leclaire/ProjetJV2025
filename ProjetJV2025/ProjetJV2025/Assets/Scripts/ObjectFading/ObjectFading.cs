using UnityEngine;
using System.Collections.Generic;
using System;
public class ObjectFading : MonoBehaviour, IEquatable<ObjectFading>
{
    public List<Renderer> Renderers = new List<Renderer>();
    public Vector3 Position;
    public List<Material> Materials = new List<Material>();
    [HideInInspector] public float InitialAlpha;

    private void Awake()
    {
        Position = transform.position;

        if (Renderers.Count == 0)
        {
            Renderers.AddRange(GetComponentsInChildren<Renderer>());
        }

        foreach (Renderer renderer in Renderers)
        {
            Materials.AddRange(renderer.materials);
        }

        InitialAlpha = Materials[0].color.a;
    }
    
    public bool Equals(ObjectFading other)
    {
        return Position.Equals(other.Position);
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }
    
}
