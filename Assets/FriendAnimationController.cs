using System.Collections;
using UnityEngine;

// Este script controla las animaciones del amigo.
// Hace que primero salude y después empiece a caminar.
public class FriendAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Ejecuta saludo y luego caminar
    public void PlayGreetingThenWalk(float delayBeforeWalk = 2.0f)
    {
        StartCoroutine(GreetingThenWalkCoroutine(delayBeforeWalk));
    }

    private IEnumerator GreetingThenWalkCoroutine(float delay)
    {
        if (animator == null) yield break;

        animator.SetBool("isWalking", false);

        yield return new WaitForSeconds(delay);

        animator.SetBool("isWalking", true);
    }
}