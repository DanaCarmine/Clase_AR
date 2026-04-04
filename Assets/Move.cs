using System.Collections;
using UnityEngine;
using Vuforia;

public class Move : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject model;
    public Animator animator;

    [Header("Movimiento")]
    public float speed = 1.5f;

    [Header("Animación")]
    public string movingBoolName = "IsMoving";

    private bool isMoving = false;

    public void MoveToCurrentRevealedTarget()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveModel());
        }
    }

    private IEnumerator MoveModel()
    {
        isMoving = true;

        if (TargetRevealManager.Instance == null)
        {
            Debug.Log("No existe TargetRevealManager.");
            isMoving = false;
            yield break;
        }

        RandomTargetContent targetContent = TargetRevealManager.Instance.GetCurrentRevealedTarget();

        if (targetContent == null)
        {
            Debug.Log("No hay ningún target revelado.");
            isMoving = false;
            yield break;
        }

        ObserverBehaviour target = targetContent.GetComponent<ObserverBehaviour>();

        if (target == null)
        {
            Debug.Log("El target revelado no tiene ObserverBehaviour.");
            isMoving = false;
            yield break;
        }

        if (!targetContent.IsVisible() || targetContent.IsCompleted())
        {
            Debug.Log("El target revelado no está disponible.");
            isMoving = false;
            yield break;
        }

        if (GameProgress.Instance != null)
        {
            bool correctOrder = GameProgress.Instance.IsExpectedContent(targetContent.contentType);

            if (!correctOrder)
            {
                GameProgress.Instance.ShowWrongContentMessage(targetContent.contentType);
                isMoving = false;
                yield break;
            }
        }

        model.transform.SetParent(null, true);

        if (animator != null)
        {
            animator.SetBool(movingBoolName, true);
        }

        Vector3 startPosition = model.transform.position;
        Vector3 endPosition = target.transform.position;

        Quaternion startRotation = model.transform.rotation;
        Quaternion endRotation = target.transform.rotation;

        float distance = Vector3.Distance(startPosition, endPosition);
        float duration = distance / speed;
        float elapsed = 0f;

        if (duration < 0.01f)
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

        if (animator != null)
        {
            animator.SetBool(movingBoolName, false);
        }

        bool success = false;

        if (GameProgress.Instance != null)
        {
            success = GameProgress.Instance.TryRegisterContent(targetContent.contentType);
        }

        if (success)
        {
            if (targetContent.contentType == TargetContentType.Stage)
            {
                targetContent.CompleteContent();

                model.SetActive(false);

                if (FinalMusicController.Instance != null)
                {
                    FinalMusicController.Instance.PlayFinalMusic();
                }
            }
            else
            {
                model.transform.SetParent(target.transform, true);

                targetContent.PlaceFriendForMeeting(model.transform);

                targetContent.CompleteFriendWithMeeting();
            }

            if (TargetRevealManager.Instance != null)
            {
                TargetRevealManager.Instance.ResetReveal();
            }
        }

        isMoving = false;
    }

    public void ResetMoveState()
    {
        StopAllCoroutines();
        isMoving = false;

        if (animator != null)
        {
            animator.SetBool(movingBoolName, false);
        }

        if (model != null)
        {
            model.SetActive(true);
        }
    }
   
}