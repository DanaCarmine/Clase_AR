using UnityEngine;

// Para que el amigo siga a Miku automáticamente.
// Si está lejos, camina hacia ella. Si está cerca, se detiene.
// También controla la animación de caminar.
public class FollowMiku : MonoBehaviour
{
    public Transform target; // A quién va a seguir (Miku)
    public float followSpeed = 1.2f; // Velocidad de seguimiento
    public float stopDistance = 0.08f; // Distancia mínima para detenerse
    public string walkingBoolName = "isWalking"; // Parámetro del Animator

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position;
        desiredPosition.y = transform.position.y;

        float distance = Vector3.Distance(transform.position, desiredPosition);

        if (distance > stopDistance)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                desiredPosition,
                followSpeed * Time.deltaTime
            );

            if (animator != null)
                animator.SetBool(walkingBoolName, true);
        }
        else
        {
            if (animator != null)
                animator.SetBool(walkingBoolName, false);
        }
    }
}