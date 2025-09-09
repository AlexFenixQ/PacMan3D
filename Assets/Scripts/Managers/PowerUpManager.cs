using System;
using System.Collections;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;
    public float defaultDuration = 8f;
    public event Action<float> OnPowerTimeChanged;
    public event Action OnPowerEnd;

    bool active;
    Coroutine co;

    void Awake() { if (Instance == null) Instance = this; else Destroy(gameObject); }

    public bool IsActive => active;

    public void Activate(float duration = -1f)
    {
        if (duration <= 0) duration = defaultDuration;
        if (co != null) StopCoroutine(co);
        co = StartCoroutine(Run(duration));
    }

    IEnumerator Run(float duration)
    {
        active = true;
        OnPowerTimeChanged?.Invoke(duration);
        float t = duration;
        while (t > 0)
        {
            yield return new WaitForSeconds(0.1f);
            t -= 0.1f;
            OnPowerTimeChanged?.Invoke(t);
        }
        active = false;
        OnPowerEnd?.Invoke();
    }
}