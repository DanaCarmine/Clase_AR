using System.Collections;
using UnityEngine;

// Este script controla todo el evento de encontrar a un amigo.
// Hace que el amigo aparezca, salude a Miku y luego empiece a seguirla.
public class FriendEncounter : MonoBehaviour
{
    public string friendName;
    public GameObject friendObject;
    public Transform mikuTransform;

    [Header("Posición en marcador")]
    public Vector3 markerOffset = new Vector3(0.25f, 0f, 0f);

    private bool visible = false;
    private bool recruited = false;

    private FriendAnimationController animController;
    private FollowMiku followScript;
    private Animator friendAnimator;

    private void Awake()
    {
        if (friendObject != null)
        {
            animController = friendObject.GetComponent<FriendAnimationController>();
            followScript = friendObject.GetComponent<FollowMiku>();
            friendAnimator = friendObject.GetComponent<Animator>();

            friendObject.SetActive(false);

            if (followScript != null)
                followScript.enabled = false;
        }
    }

    // Hace aparecer al amigo en el marcador
    public void ShowFriend()
    {
        if (visible || friendObject == null) return;

        visible = true;
        friendObject.SetActive(true);

        friendObject.transform.SetParent(transform, false);
        friendObject.transform.localPosition = markerOffset;
        friendObject.transform.localRotation = Quaternion.identity;
    }

    // Inicia la secuencia para unir al amigo con Miku
    public void RecruitFriend()
    {
        if (recruited || friendObject == null) return;

        recruited = true;
        StartCoroutine(RecruitSequence());
    }

    // Secuencia completa: girar → saludar → seguir a Miku
    private IEnumerator RecruitSequence()
    {
        friendObject.transform.SetParent(null, true);

        yield return StartCoroutine(LookAtMikuSafe());

        if (friendAnimator != null)
            friendAnimator.SetBool("isWalking", false);

        if (animController != null)
            animController.PlayGreetingThenWalk(2f);

        yield return new WaitForSeconds(2.1f);

        if (followScript != null)
            followScript.enabled = true;
    }

    // Hace que el amigo gire suavemente hacia Miku
    private IEnumerator LookAtMikuSafe()
    {
        if (mikuTransform == null || friendObject == null) yield break;

        Vector3 lookDir = mikuTransform.position - friendObject.transform.position;
        lookDir.y = 0f;

        if (lookDir.magnitude < 0.25f)
            yield break;

        Quaternion startRot = friendObject.transform.rotation;
        Quaternion endRot = Quaternion.LookRotation(lookDir.normalized);

        float duration = 0.35f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            friendObject.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        friendObject.transform.rotation = endRot;
    }

    public bool IsVisible()
    {
        return visible;
    }

    public bool IsRecruited()
    {
        return recruited;
    }
}