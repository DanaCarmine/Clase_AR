using System.Collections;
using UnityEngine;

public class FriendAnimationController : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Parámetros")]
    public string idleBoolName = "isWalking";
    public string greetingTriggerName = "Greeting";

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void PlayGreeting(float greetingDuration = 2f)
    {
        StopAllCoroutines();
        StartCoroutine(GreetingCoroutine(greetingDuration));
    }

    private IEnumerator GreetingCoroutine(float duration)
    {
        if (animator == null) yield break;

        if (!string.IsNullOrEmpty(idleBoolName))
            animator.SetBool(idleBoolName, false);

        if (!string.IsNullOrEmpty(greetingTriggerName))
            animator.SetTrigger(greetingTriggerName);

        yield return new WaitForSeconds(duration);
    }
}