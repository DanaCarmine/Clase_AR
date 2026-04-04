using UnityEngine;
using Vuforia;

public class TargetStatusRouter : MonoBehaviour
{
    [Header("Referencias")]
    public ObserverBehaviour observerBehaviour;
    public RandomTargetContent randomTargetContent;

    private bool wasTracked = false;

    private void Reset()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();
        randomTargetContent = GetComponent<RandomTargetContent>();
    }

    private void OnEnable()
    {
        if (observerBehaviour == null)
            observerBehaviour = GetComponent<ObserverBehaviour>();

        if (randomTargetContent == null)
            randomTargetContent = GetComponent<RandomTargetContent>();

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
        bool isTrackedNow = IsTrackedForGameplay(status);

        if (isTrackedNow && !wasTracked)
        {
            wasTracked = true;

            if (ARVisibilityManager.Instance != null)
            {
                ARVisibilityManager.Instance.RegisterTargetVisible(gameObject.name);
            }

            if (randomTargetContent != null)
            {
                randomTargetContent.OnTargetDetected();
            }
        }
        else if (!isTrackedNow && wasTracked)
        {
            wasTracked = false;

            if (ARVisibilityManager.Instance != null)
            {
                ARVisibilityManager.Instance.RegisterTargetLost(gameObject.name);
            }

            if (randomTargetContent != null)
            {
                randomTargetContent.OnTargetLost();
            }
        }
    }

    private bool IsTrackedForGameplay(TargetStatus status)
    {
        return status.Status == Status.TRACKED;
    }
}