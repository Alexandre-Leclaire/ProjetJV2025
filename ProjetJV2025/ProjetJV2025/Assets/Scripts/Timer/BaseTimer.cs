using UnityEngine;
using UnityEngine.Events;

public class BaseTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float duration = 10f;  // durée en secondes
    protected float timer;
    protected bool isRunning = false;

    [Header("Timer Events")]
    public UnityEvent onTimerEnd;

    public virtual void StartTimer()
    {
        timer = duration;
        isRunning = true;
        Debug.Log($"{gameObject.name} : Timer démarré ({duration} sec)");
    }

    protected virtual void Update()
    {
        if (isRunning)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                EndTimer();
            }
        }
    }

    protected virtual void EndTimer()
    {
        isRunning = false;
        Debug.Log($"{gameObject.name} : Timer terminé !");
        onTimerEnd?.Invoke();
    }
}
