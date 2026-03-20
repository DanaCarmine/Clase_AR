using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Move : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject model;
    public ObserverBehaviour[] imageTargets;
    public Animator animator;

    [Header("Movimiento")]
    public float speed = 1.5f;
    public float stopDistance = 0.01f;

    [Header("Animación")]
    public string idleBoolName = "IsMoving";

    private bool isMoving = false;
    private int lastTargetIndex = -1;

    public void MoveToRandomMarker()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveModel());
        }
    }

    private IEnumerator MoveModel()
    {
        isMoving = true;

        // Para que se despegue del marcador 1
        model.transform.parent = null;

        ObserverBehaviour target = GetRandomDetectedTarget();

        if (target == null)
        {
            isMoving = false;
            yield break;
        }

        // Activar caminar
        if (animator != null)
        {
            animator.SetBool(idleBoolName, true);
        }

        Vector3 startPosition = model.transform.position;
        Vector3 endPosition = target.transform.position;

        Quaternion startRotation = model.transform.rotation;
        Quaternion endRotation = target.transform.rotation;

        float distance = Vector3.Distance(startPosition, endPosition);
        float duration = distance / speed;
        float elapsed = 0f;

        if (duration <= 0.01f)
            duration = 0.01f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            model.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            model.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);

            yield return null;
        }

        model.transform.position = endPosition;
        model.transform.rotation = endRotation;

        // Detener caminar al llegar
        if (animator != null)
        {
            animator.SetBool(idleBoolName, false);
        }

        isMoving = false;
    }

    private ObserverBehaviour GetRandomDetectedTarget()
    {
        List<int> availableIndexes = new List<int>();

        for (int i = 0; i < imageTargets.Length; i++)
        {
            ObserverBehaviour target = imageTargets[i];

            if (target != null &&
                (target.TargetStatus.Status == Status.TRACKED ||
                 target.TargetStatus.Status == Status.EXTENDED_TRACKED))
            {
                availableIndexes.Add(i);
            }
        }

        if (availableIndexes.Count == 0)
            return null;

        if (availableIndexes.Count > 1 && availableIndexes.Contains(lastTargetIndex))
        {
            availableIndexes.Remove(lastTargetIndex);
        }

        int randomIndex = availableIndexes[Random.Range(0, availableIndexes.Count)];
        lastTargetIndex = randomIndex;

        return imageTargets[randomIndex];
    }
}