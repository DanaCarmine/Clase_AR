using UnityEngine;

public class TargetRevealManager : MonoBehaviour
{
    public static TargetRevealManager Instance;

    private RandomTargetContent currentRevealedTarget;

    private void Awake()
    {
        Instance = this;
    }

    public bool TryReveal(RandomTargetContent target)
    {
        if (target == null) return false;

        // Si el target actual ya no está visible, lo liberamos
        if (currentRevealedTarget != null && !currentRevealedTarget.IsVisible())
        {
            currentRevealedTarget = null;
        }

        // Si no hay ninguno revelado
        if (currentRevealedTarget == null)
        {
            currentRevealedTarget = target;
            target.RevealContent();
            return true;
        }

        // Si es el mismo target
        if (currentRevealedTarget == target)
        {
            target.RevealContent();
            return true;
        }

        // Si hay otro revelado
        return false;
    }

    public void NotifyTargetLost(RandomTargetContent target)
    {
        if (currentRevealedTarget == target)
        {
            currentRevealedTarget = null;
        }
    }

    public RandomTargetContent GetCurrentRevealedTarget()
    {
        if (currentRevealedTarget != null && !currentRevealedTarget.IsVisible())
        {
            currentRevealedTarget = null;
        }

        return currentRevealedTarget;
    }

    public void ResetReveal()
    {
        currentRevealedTarget = null;
    }
}