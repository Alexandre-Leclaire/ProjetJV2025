using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(PlayerInput))]
public class ActionBox : MonoBehaviour
{
    [Header("UI")]
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

    string idleMessage = "Press E to interact";

    void Start()
    {
        if (canvas == null || actionNameText == null || bottomText == null)
        {
            Debug.LogError($"[ActionBox] UI references missing on {name}");
            enabled = false;
            return;
        }

        actionnable = GetComponent<IActionnable>();
        if (actionnable == null)
        {
            Debug.LogError($"[ActionBox] No IActionnable found on {name}");
            enabled = false;
            return;
        }

        actions = actionnable.GetActions();
        if (actions == null || actions.Count == 0)
        {
            Debug.LogError($"[ActionBox] No actions defined on {name}");
            enabled = false;
            return;
        }

        input = GetComponent<PlayerInput>();
        input.enabled = false;

        canvas.SetActive(false);
        curIndex = 0;
        UpdateText();
        PlaceCanvas();
    }

    void PlaceCanvas()
    {
        canvas.transform.localPosition = new Vector3(0f, 1.2f, 0f);
        canvas.transform.localRotation = Quaternion.identity;
    }

    void Update()
    {
        if (resetCooldown > 0f)
        {
            resetCooldown -= Time.deltaTime;
            bottomText.text = $"Usable in {(int)resetCooldown}s...";
            return;
        }

        if (player != null && Input.GetKeyDown(KeyCode.E))
        {
            TrySubmit();
        }
    }

    void TrySubmit()
    {
        if (player == null || doingAction || resetCooldown > 0f)
            return;

        StartCoroutine(DoAction(player));
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        player = other.gameObject;
        input.enabled = true;

        canvas.SetActive(true);
        LookAtCamera();

        bottomText.text = idleMessage;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        canvas.SetActive(false);
        input.enabled = false;
        player = null;

        StopAllCoroutines();
        doingAction = false;
    }

    void LookAtCamera()
    {
        canvas.transform.LookAt(
            canvas.transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up
        );
    }

    IEnumerator DoAction(GameObject player)
    {
        doingAction = true;
        IActionnable.Element action = actions[curIndex];

        // ❌ FAIL → DEBUG ONLY
        if (!action.onInteract(player))
        {
            Debug.LogWarning(
                $"[ActionBox] Action '{action.name}' failed on '{name}' (missing resources)"
            );

            doingAction = false;
            yield break;
        }

        // ⏳ Timer
        float time = action.interactionTime;
        while (time > 0f)
        {
            bottomText.text = $"Finish in {(int)time}s...";
            time -= Time.deltaTime;
            yield return null;
        }

        // ✅ SUCCESS
        action.onInteractionFinish(player);
        Debug.Log($"[ActionBox] Action '{action.name}' completed on '{name}'");

        resetCooldown = action.resetCooldown;
        bottomText.text = idleMessage;

        doingAction = false;
    }

    void UpdateText()
    {
        actionNameText.text = actions[curIndex].name;
    }
}
