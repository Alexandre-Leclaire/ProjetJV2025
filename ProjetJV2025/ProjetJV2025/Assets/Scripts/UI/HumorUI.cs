using TMPro;
using UnityEngine;

public class HumorUI : MonoBehaviour
{
    TMP_Text text;
    PeriodController periodController;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
        if (text == null)
        {
            Debug.LogError("[HumorUI] No TMP_Text found");
        }

        periodController = FindFirstObjectByType<PeriodController>();
        if (periodController == null)
        {
            Debug.LogError("[HumorUI] No PeriodController found");
        }
    }

    void Update()
    {
        if (text == null || periodController == null)
            return;

        text.text = $"Period :\n{periodController.CurrentPeriod}";
    }
}
