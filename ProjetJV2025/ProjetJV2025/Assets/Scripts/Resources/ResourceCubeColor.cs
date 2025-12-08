using UnityEngine;

public class ResourceCubeColor : MonoBehaviour
{
    public void SetColor(Item.ItemType type)
    {
        var rend = GetComponent<Renderer>();

        switch (type)
        {
            case Item.ItemType.Food:
                rend.material.color = Color.green;
                break;

            case Item.ItemType.Cloth:
                rend.material.color = Color.cyan;
                break;

            case Item.ItemType.Metal:
                rend.material.color = Color.gray;
                break;
        }
    }
}
