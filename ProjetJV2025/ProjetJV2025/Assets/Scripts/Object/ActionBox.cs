using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(IActionnable))]
[RequireComponent(typeof(PlayerInput))]
public class ActionBox : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private TMP_Text actionNameText;
    [SerializeField] private TMP_Text bottomText;

    private int curIndex;
    private bool doingAction;
    private float resetCooldown;
    private GameObject player;
    private PlayerInput input;

    private List<IActionnable.Element> actions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actions = GetComponent<IActionnable>().GetActions();
        input = GetComponent<PlayerInput>();
        input.enabled = false;

        canvas.SetActive(false);
        UpdateText();
    }

    void Update()
    {
        if (resetCooldown > 0f)
        {
            bottomText.text = $"Usable in {(int)resetCooldown}s...";
            resetCooldown -= Time.deltaTime;
        }
    }

    void OnSubmit()
    {
        Debug.Log($"Submit {player} && {doingAction} && {resetCooldown}");
        if (player != null && !doingAction && resetCooldown <= 0f)
        {
            doingAction = true;
            StartCoroutine(DoAction(player));
        }
    }

    void OnNavigate(InputValue value)
    {
        if (player != null)
        {
            int m = (int)value.Get<float>();

            curIndex = (curIndex + m) % actions.Count;
            if (curIndex < 0)
            {
                curIndex = actions.Count - 1;
            }
            UpdateText();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            input.enabled = true;
            canvas.SetActive(true);
            canvas.transform.LookAt(Camera.main.transform);
        }
    }

    private IEnumerator DoAction(GameObject player)
    {
        IActionnable.Element action = actions[curIndex];

        if (action.onInteract(player))
        {
            float time = action.interactionTime;
            while (time > 0f)
            {
                bottomText.text = $"Finish in {(int)time}s...";
                time -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            action.onInteractionFinish(player);
        }

        resetCooldown = action.resetCooldown;
        doingAction = false;
    }

    private void UpdateText()
    {
        actionNameText.text = actions[curIndex].name;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
    }
}