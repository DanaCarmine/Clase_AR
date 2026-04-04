using UnityEngine;
using Vuforia;

public class BasicTargetStatusReporter : MonoBehaviour
{
    public ObserverBehaviour observerBehaviour;

    private bool wasTracked = false;

    private void Reset()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();
    }

    private void OnEnable()
    {
        if (observerBehaviour == null)
            observerBehaviour = GetComponent<ObserverBehaviour>();

        if (observerBehaviour != null)
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
    }

    private void OnDisable()
    {
        if (observerBehaviour != null)
            observerBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        bool isTrackedNow = status.Status == Status.TRACKED;

        if (isTrackedNow && !wasTracked)
        {
            wasTracked = true;

            if (ARVisibilityManager.Instance != null)
            {
                ARVisibilityManager.Instance.RegisterTargetVisible(gameObject.name);
            }
        }
        else if (!isTrackedNow && wasTracked)
        {
            wasTracked = false;

            if (ARVisibilityManager.Instance != null)
            {
                ARVisibilityManager.Instance.RegisterTargetLost(gameObject.name);
            }
        }
    }
}