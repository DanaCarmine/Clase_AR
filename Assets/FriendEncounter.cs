using System.Collections;
using UnityEngine;

public class FriendEncounter : MonoBehaviour
{
    public string friendName;
    public GameObject friendObject;

    [Header("Posición sobre marcador")]
    public Vector3 markerOffset = new Vector3(0.2f, 0f, 0f);

    [Header("Tiempo de despedida")]
    public float greetingDuration = 2f;
    public float disappearDelay = 0.5f;

    private bool visible = false;
    private bool recruited = false;

    private FriendAnimationController animController;

    private void Awake()
    {
        if (friendObject != null)
        {
            animController = friendObject.GetComponent<FriendAnimationController>();
            friendObject.SetActive(false);
        }
    }

    public void ShowFriend()
    {
        if (visible || friendObject == null) return;

        visible = true;
        friendObject.SetActive(true);

        friendObject.transform.SetParent(transform, false);
        friendObject.transform.localPosition = markerOffset;
        friendObject.transform.localRotation = Quaternion.identity;
    }

    public void RecruitFriend()
    {
        if (recruited || friendObject == null) return;

        recruited = true;
        StartCoroutine(RecruitSequence());
    }

    private IEnumerator RecruitSequence()
    {
        if (animController != null)
        {
            animController.PlayGreeting(greetingDuration);
        }

        yield return new WaitForSeconds(greetingDuration + disappearDelay);

        friendObject.SetActive(false);
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