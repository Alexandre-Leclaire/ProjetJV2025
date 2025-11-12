using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Enumerable = System.Linq.Enumerable;

public class OcclusionMasking : MonoBehaviour
{
    [SerializeField] 
    private Transform targetObject;
    
    [SerializeField] 
    private LayerMask layerMask;
    
    [SerializeField]
    private Camera camera;

    [SerializeField]
    private RaycastHit[] hitBuffer;
    private void Awake()
    {
        camera = GetComponent<Camera>();
    }
    
    private void Update()
    {
        Vector2 cutoutPos = camera.WorldToViewportPoint(targetObject.position);
        cutoutPos.y /= (Screen.width / Screen.height);
        
            
        Vector3 offset = targetObject.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, layerMask);
        //hitBuffer.AddRange(hitObjects);
        // Il faut garder en memoire les objets qui ont le shader applique, si jamais ils ne sont plus dans l'iteration actuelle il faut remettre les parametres par defaut
        // + Ajouter un gameObject devant le joueur pour forcer le shader avant que le joueur l'active pour une meilleure transition
        
        for (int i = 0; i < hitObjects.Length; i++)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;
            for (int j = 0; j < materials.Length; j++)
            {
                materials[j].SetVector("_CutoutPos", cutoutPos);
                materials[j].SetFloat("_CutoffSize", 0.20f);
                materials[j].SetFloat("_FalloffSize", 0.05f);
            }
        }
    }
    
}
