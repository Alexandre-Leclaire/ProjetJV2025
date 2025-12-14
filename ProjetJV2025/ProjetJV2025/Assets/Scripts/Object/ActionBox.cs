using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(PlayerInput))]
public class ActionBox : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private TMP_Text actionNameText;
    [SerializeField] private TMP_Text bottomText;

    int curIndex;
    bool doingAction;
    float resetCooldown;
    GameObject player;
    PlayerInput input;

    IActionnable actionnable;
    List<IActionnable.Element> actions;

    void Start()
    {
        if (canvas == null || actionNameText == null || bottomText == null)
        {
            Debug.LogError($"ActionBox ({name}) : références UI manquantes");
            enabled = false;
            return;
        }

        actionnable = GetComponent<IActionnable>();
        if (actionnable == null)
        {
            Debug.LogError($"ActionBox ({name}) : aucun composant IActionnable trouvé");
            enabled = false;
            return;
        }

        actions = actionnable.GetActions();
        if (actions == null || actions.Count == 0)
        {
            Debug.LogError($"ActionBox ({name}) : aucune action définie");
            enabled = false;
            return;
        }

        input = GetComponent<PlayerInput>();
        input.enabled = false;

        canvas.SetActive(false);
        curIndex = 0;
        UpdateText();
    }

    void Update()
    {
        if (resetCooldown > 0f)
        {
            bottomText.text = $"Usable in {(int)resetCooldown}s...";
            resetCooldown -= Time.deltaTime;
        }
        else
        {
            bottomText.text = "C";
        }
    }

    void OnNavigate(InputValue value)
    {
        int v = (int)value.Get<float>();
        
        curIndex = (curIndex + v) % actions.Count;
        if (curIndex < 0)
        {
            curIndex = actions.Count - 1; 
        }

        UpdateText();
    }

    void OnSubmit()
    {
        if (player == null || doingAction || resetCooldown > 0f)
            return;

        doingAction = true;
        StartCoroutine(DoAction(player));
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        player = other.gameObject;
        input.enabled = true;
        canvas.SetActive(true);

        canvas.transform.LookAt(Camera.main.transform);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        canvas.SetActive(false);
        input.enabled = false;
        player = null;

        if (doingAction && actions[curIndex].cancelOnLeave)
        {
            doingAction = false;
            resetCooldown = actions[curIndex].resetCooldown;
            StopAllCoroutines();
        }
    }

    IEnumerator DoAction(GameObject player)
    {
        IActionnable.Element action = actions[curIndex];

        if (action.onInteract(player))
        {
            float time = action.interactionTime;
            while (time > 0f)
            {
                bottomText.text = $"Finish in {(int)time}s...";
                time -= Time.deltaTime;
                yield return null;
            }

            action.onInteractionFinish(player);
            resetCooldown = action.resetCooldown;
        }
        
        doingAction = false;
    }

    void UpdateText()
    {
        actionNameText.text = actions[curIndex].name;
    }
}
