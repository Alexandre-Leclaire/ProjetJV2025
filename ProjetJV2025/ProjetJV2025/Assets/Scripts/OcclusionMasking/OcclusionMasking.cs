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
    private List<RaycastHit> hitBuffer = new List<RaycastHit>();
    
    [SerializeField]
    private GameObject sphereTrigger;
    private void Awake()
    {
        camera = GetComponent<Camera>();
    }
    
    private void Update()
    {
        Vector2 cutoutPos = camera.WorldToViewportPoint(targetObject.position);
        cutoutPos.y /= (Screen.width / Screen.height);
        
        Vector3 offset = targetObject.position - transform.position;
        RaycastHit[] occludingObjects = Physics.SphereCastAll(transform.position, 1.20f, transform.forward, offset.magnitude - 2f, layerMask);
        //Debug.Log("1:" + occludingObjects.Length);
        if (hitBuffer != null && hitBuffer.Count > 0)
        {
            RaycastHit[] resetObjects = hitBuffer.Union(occludingObjects).ToArray();
            foreach (var obj in resetObjects)
            {
                Material[] mats = obj.transform.GetComponent<Renderer>().materials;
                for (int j = 0; j < mats.Length; j++)
                {
                    mats[j].SetFloat("_CutoffSize", 0f);
                    mats[j].SetFloat("_FalloffSize", 0f);
                    mats[j].SetOverrideTag("RenderType", "Opaque");
                    mats[j].SetFloat("_Alpha", 1f);
                    mats[j].SetFloat("_Mode", 1f);
                }
            }
            //On reset le buffer apres avoir reset les shaders d'objets
            hitBuffer.Clear();
        }
        
        //Debug.Log(occludingObjects);
        
        // Il faut garder en memoire les objets qui ont le shader applique, si jamais ils ne sont plus dans l'iteration actuelle il faut remettre les parametres par defaut
        // + Ajouter un gameObject devant le joueur pour forcer le shader avant que le joueur l'active pour une meilleure transition
        
        for (int i = 0; i < occludingObjects.Length; i++)
        {
            
            Material[] materials = occludingObjects[i].transform.GetComponent<Renderer>().materials;
            for (int j = 0; j < materials.Length; j++)
            {
                materials[j].SetVector("_CutoutPos", cutoutPos);
                materials[j].SetFloat("_CutoffSize", 0.15f);
                materials[j].SetFloat("_FalloffSize", 0.05f);
                materials[j].SetFloat("_Alpha", 0.4f);
                materials[j].SetOverrideTag("RenderType", "Transparent");
                materials[j].SetFloat("_Mode", 2f);
            }
        }
        hitBuffer.AddRange(occludingObjects);
        //Debug.Log("2:"+hitBuffer.Count);
        
    }

}
