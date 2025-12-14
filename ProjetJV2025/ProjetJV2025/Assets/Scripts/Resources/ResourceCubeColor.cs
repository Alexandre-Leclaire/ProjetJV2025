using UnityEngine;

public class ResourceCubeColor : MonoBehaviour
{
    public void SetColor(string resourceId)
    {
        var rend = GetComponent<Renderer>();

        rend.material.color = resourceId switch
        {
            "Food" => Color.red,
            "Cloth" => Color.white,
            "Metal" => Color.black,
            _ => Color.blue
        };
    }
}
