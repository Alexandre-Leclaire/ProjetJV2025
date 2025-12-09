using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Text;
using UnityEditor.Rendering;

public class FadeObjectBlocking : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;
    
    [SerializeField] 
    private Transform target;

    [SerializeField] 
    private Camera Camera;
    
    [SerializeField] 
    [Range(0f, 1f)]
    private float fadedAlpha = 0.33f;
    
    [SerializeField] 
    private bool retainShadows = true;

    [SerializeField] 
    private Vector3 targetPositionOffset = Vector3.up;

    [SerializeField] 
    private float fadeSpeed = 1f;

    [Header("Read Only")] 
    [SerializeField]
    private List<ObjectFading> objectsBlockingView = new List<ObjectFading>();
    private Dictionary<ObjectFading, Coroutine> runningCoroutines = new Dictionary<ObjectFading, Coroutine>();
    
    private RaycastHit[] hits = new RaycastHit[10];

    private void Start()
    {
        StartCoroutine(CheckForObjects());

    }

    private IEnumerator CheckForObjects()
    {
        while (true)
        {


            int hitsnb = Physics.RaycastNonAlloc(Camera.transform.position,
                (target.transform.position + targetPositionOffset - Camera.transform.position).normalized,
                hits,
                Vector3.Distance(Camera.transform.position,
                    target.transform.position + targetPositionOffset),
                layerMask);
            if (hitsnb > 0)
            {
                for (int i = 0; i < hitsnb; i++)
                {
                    ObjectFading objectFading = GetFadingObjectFromHit(hits[i]);
                    if (objectFading != null && !objectsBlockingView.Contains(objectFading))
                    {
                        if (runningCoroutines.ContainsKey(objectFading))
                        {
                            if (runningCoroutines[objectFading] != null)
                            {
                                StopCoroutine(runningCoroutines[objectFading]);
                            }

                            runningCoroutines.Remove(objectFading);
                        }

                        runningCoroutines.Add(objectFading, StartCoroutine(FadeObjectOut(objectFading)));
                        objectsBlockingView.Add(objectFading);
                    }
                }
            }

            FadeObjectNoLongerBlocking();
            ClearHits();
            yield return null;
        }
    }

    private void ClearHits()
    {
        System.Array.Clear(hits, 0, hits.Length);
    }

    private IEnumerator FadeObjectOut(ObjectFading objectFading)
    {
        foreach (Material material in objectFading.Materials)
        {
            material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.SetInt("_Surface", 1);
            
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            
            material.SetShaderPassEnabled("DepthOnly", false);
            material.SetShaderPassEnabled("SHADOWCASTER", retainShadows);
            
            material.SetOverrideTag("RenderType", "Transparent");
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        }

        float time = 0f;
        while (objectFading.Materials[0].color.a < fadedAlpha)
        {
            foreach (Material material in objectFading.Materials)
            {
                if (material.HasProperty("_Color"))
                {
                    material.color = new Color(
                        material.color.r, material.color.g, material.color.b,
                        Mathf.Lerp(objectFading.InitialAlpha,
                            fadedAlpha,
                            time * fadeSpeed)
                        );
                }
            }
            time += Time.deltaTime;
            yield return null;
        }

        if (runningCoroutines.ContainsKey(objectFading))
        {
            StopCoroutine(runningCoroutines[objectFading]);
            runningCoroutines.Remove(objectFading);
        }
    }

    private IEnumerator FadeObjectIn(ObjectFading objectFading)
    {
        float time = 0f;
        while (objectFading.Materials[0].color.a < fadedAlpha)
        {
            foreach (Material material in objectFading.Materials)
            {
                if (material.HasProperty("_Color"))
                {
                    material.color = new Color(
                        material.color.r, material.color.g, material.color.b,
                        Mathf.Lerp(fadedAlpha, objectFading.InitialAlpha,
                            time * fadeSpeed)
                    );
                }
            }
            time += Time.deltaTime;
            yield return null;
        }
        foreach (Material material in objectFading.Materials)
        {
            material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.SetInt("_Surface", 0);
            
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
            
            material.SetShaderPassEnabled("DepthOnly", true);
            material.SetShaderPassEnabled("SHADOWCASTER", true);
            
            material.SetOverrideTag("RenderType", "Opaque");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
        }

        if (runningCoroutines.ContainsKey(objectFading))
        {
            StopCoroutine(runningCoroutines[objectFading]);
            runningCoroutines.Remove(objectFading);
        }
    }

    private void FadeObjectNoLongerBlocking()
    {
        List<ObjectFading> objectsToRemove = new List<ObjectFading>(objectsBlockingView.Count);

        foreach (ObjectFading objectFading in objectsBlockingView)
        {
            bool objectIsBlocking = false;
            for (int i = 0; i < hits.Length; i++)
            {
                ObjectFading blockFadingObject = GetFadingObjectFromHit(hits[i]);
                if (blockFadingObject != null && objectFading == blockFadingObject)
                {
                    objectIsBlocking = true;
                    break;
                }
            }

            if (!objectIsBlocking)
            {
                if (runningCoroutines.ContainsKey(objectFading))
                {
                    if (runningCoroutines[objectFading] != null)
                    {
                        StopCoroutine(runningCoroutines[objectFading]);
                    }
                    runningCoroutines.Remove(objectFading);
                }
                runningCoroutines.Add(objectFading, StartCoroutine(FadeObjectIn(objectFading)));
                objectsToRemove.Add(objectFading);
            }
                
        }

        foreach (ObjectFading removeObject in  objectsToRemove)
        {
           objectsBlockingView.Remove(removeObject); 
        }
        
    }
    
    
    private ObjectFading GetFadingObjectFromHit(RaycastHit hit)
    {
        return hit.collider != null ? hit.collider.GetComponent<ObjectFading>() : null;
    }
}
