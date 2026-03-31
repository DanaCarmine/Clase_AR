using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

// Este script mueve a Miku entre distintos marcadores detectados por Vuforia.
// También verifica si hay amigos en el marcador y los recluta.
public class Move : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject model;
    public ObserverBehaviour[] imageTargets;
    public Animator animator;

    [Header("Movimiento")]
    public float speed = 1.5f;

    [Header("Animación")]
    public string movingBoolName = "IsMoving";

    private bool isMoving = false;
    private int lastTargetIndex = -1;

    // Se activa al presionar un botón
    public void MoveToRandomMarker()
    {
        Debug.Log("BOTON MOVE SI FUE PRESIONADO");

        if (!isMoving)
        {
            StartCoroutine(MoveModel());
        }
    }

    // Movimiento principal de Miku
    private IEnumerator MoveModel()
    {
        isMoving = true;

        ObserverBehaviour target = GetRandomDetectedTarget();

        if (target == null)
        {
            Debug.Log("No se encontró ningún marcador válido");
            isMoving = false;
            yield break;
        }

        // Se separa del marcador actual
        // null = sin padre (independiente)
        // true = mantiene su posición global (no se mueve de golpe)
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

        // Se vuelve hijo del nuevo marcador
        // Esto hace que Miku se mueva junto con el marcador
        // true = conserva su posición actual en el mundo
        model.transform.SetParent(target.transform, true);

        if (animator != null)
        {
            animator.SetBool(movingBoolName, false);
        }

        // Verifica si hay amigo en ese marcador
        FriendEncounter encounter = target.GetComponent<FriendEncounter>();

        if (encounter != null && encounter.IsVisible() && !encounter.IsRecruited())
        {
            encounter.RecruitFriend();

            if (GameProgress.Instance != null)
            {
                GameProgress.Instance.RegisterFriend(encounter.friendName);
            }
        }

        isMoving = false;
    }

    // Obtiene un marcador detectado al azar
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