using UnityEngine;

public class ResourceCubeColor : MonoBehaviour
{
    public void SetColor(string resourceId)
    {
        var rend = GetComponent<Renderer>();

        rend.material.color = resourceId switch
        {
            "Food" => Color.green,
            "Cloth" => Color.cyan,
            "Metal" => Color.gray,
            _ => Color.white
        };
    }
}
