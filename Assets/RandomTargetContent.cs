using System.Collections;
using UnityEngine;

public class RandomTargetContent : MonoBehaviour
{
    [Header("Contenido asignado")]
    public TargetContentType contentType;
    public string contentName;
    public GameObject contentObject;

    [Header("Objeto tapa / carta cerrada")]
    public GameObject coverObject;

    [Header("Pool de contenido")]
    public Transform contentPool;

    [Header("Transform del contenido en el marcador")]
    public Vector3 markerOffset = new Vector3(0.2f, 0f, 0f);
    public Vector3 markerRotation = Vector3.zero;
    public Vector3 markerScale = Vector3.one;

    [Header("Encuentro con Miku")]
    public Vector3 friendMeetingOffset = new Vector3(0.35f, 0f, 0f);
    public Vector3 friendMeetingRotation = Vector3.zero;

    [Header("Transform opcional de tapa")]
    public bool useManualCoverTransform = false;
    public Vector3 coverOffset = Vector3.zero;
    public Vector3 coverRotation = Vector3.zero;
    public Vector3 coverScale = Vector3.one;

    [Header("Tiempos")]
    public float greetingDuration = 2f;
    public float disappearDelay = 0.5f;

    private bool visible = false;
    private bool completed = false;
    private bool revealed = false;

    private FriendAnimationController animController;

    private Vector3 savedCoverLocalPosition;
    private Quaternion savedCoverLocalRotation;
    private Vector3 savedCoverLocalScale;

    private void Awake()
    {
        if (coverObject != null)
        {
            savedCoverLocalPosition = coverObject.transform.localPosition;
            savedCoverLocalRotation = coverObject.transform.localRotation;
            savedCoverLocalScale = coverObject.transform.localScale;
            coverObject.SetActive(false);
        }

        if (contentObject != null)
        {
            contentObject.SetActive(false);
        }
    }

    public void SetupContent(TargetContentType newType, string newName, GameObject newObject)
    {
        if (contentObject != null)
        {
            contentObject.SetActive(false);

            if (contentPool != null)
                contentObject.transform.SetParent(contentPool, true);
        }

        if (coverObject != null)
        {
            coverObject.SetActive(false);
        }

        contentType = newType;
        contentName = newName;
        contentObject = newObject;

        visible = false;
        completed = false;
        revealed = false;
        animController = null;

        if (contentObject != null)
        {
            contentObject.SetActive(false);

            if (contentPool != null)
                contentObject.transform.SetParent(contentPool, true);

            animController = contentObject.GetComponent<FriendAnimationController>();
        }
    }

    public void OnTargetDetected()
    {
        visible = true;

        if (completed)
        {
            if (contentType == TargetContentType.Stage)
            {
                RevealContent();
            }
            else
            {
                HideAll();
            }
            return;
        }

        if (TargetRevealManager.Instance != null)
        {
            bool success = TargetRevealManager.Instance.TryReveal(this);

            if (!success)
            {
                ShowCoverOnly();
            }
        }
        else
        {
            RevealContent();
        }
    }

    public void OnTargetLost()
    {
        visible = false;
        revealed = false;

        HideAll();

        if (TargetRevealManager.Instance != null)
        {
            TargetRevealManager.Instance.NotifyTargetLost(this);
        }
    }

    public void ShowCoverOnly()
    {
        revealed = false;

        if (contentObject != null)
        {
            contentObject.SetActive(false);

            if (contentPool != null)
                contentObject.transform.SetParent(contentPool, true);
        }

        if (coverObject != null)
        {
            coverObject.transform.SetParent(transform, false);

            if (useManualCoverTransform)
            {
                coverObject.transform.localPosition = coverOffset;
                coverObject.transform.localEulerAngles = coverRotation;
                coverObject.transform.localScale = coverScale;
            }
            else
            {
                coverObject.transform.localPosition = savedCoverLocalPosition;
                coverObject.transform.localRotation = savedCoverLocalRotation;
                coverObject.transform.localScale = savedCoverLocalScale;
            }

            coverObject.SetActive(true);
        }
    }

    public void RevealContent()
    {
        if (contentObject == null) return;

        revealed = true;

        if (coverObject != null)
            coverObject.SetActive(false);

        contentObject.transform.SetParent(transform, false);
        contentObject.transform.localPosition = markerOffset;
        contentObject.transform.localEulerAngles = markerRotation;
        contentObject.transform.localScale = markerScale;
        contentObject.SetActive(true);
    }

    public void PlaceFriendForMeeting(Transform mikuTransform)
    {
        if (contentObject == null) return;

        contentObject.transform.SetParent(transform, false);
        contentObject.transform.localPosition = friendMeetingOffset;
        contentObject.transform.localEulerAngles = friendMeetingRotation;
        contentObject.transform.localScale = markerScale;
        contentObject.SetActive(true);
    }

    public void CompleteContent()
    {
        if (completed || contentObject == null) return;

        completed = true;

        if (contentType == TargetContentType.Stage)
        {
            if (coverObject != null)
                coverObject.SetActive(false);

            RevealContent();
            return;
        }

        StartCoroutine(CompleteFriendSequence());
    }

    public void CompleteFriendWithMeeting()
    {
        if (completed || contentObject == null) return;

        completed = true;
        StartCoroutine(CompleteFriendMeetingSequence());
    }

    private IEnumerator CompleteFriendMeetingSequence()
    {
        yield return null;

        if (animController != null)
        {
            animController.PlayGreeting(greetingDuration);
        }

        yield return new WaitForSeconds(greetingDuration + disappearDelay);

        if (contentObject != null)
        {
            contentObject.SetActive(false);

            if (contentPool != null)
            {
                contentObject.transform.SetParent(contentPool, true);
            }
        }

        if (coverObject != null)
        {
            coverObject.SetActive(false);
        }

        revealed = false;
    }

    private IEnumerator CompleteFriendSequence()
    {
        if (animController != null)
        {
            animController.PlayGreeting(greetingDuration);
        }

        yield return new WaitForSeconds(greetingDuration + disappearDelay);

        if (contentObject != null)
        {
            contentObject.SetActive(false);

            if (contentPool != null)
            {
                contentObject.transform.SetParent(contentPool, true);
            }
        }

        if (coverObject != null)
            coverObject.SetActive(false);

        revealed = false;
    }

    public void HideAll()
    {
        if (contentObject != null)
        {
            contentObject.SetActive(false);

            if (contentPool != null)
                contentObject.transform.SetParent(contentPool, true);
        }

        if (coverObject != null)
        {
            coverObject.SetActive(false);
        }
    }

    public void ForceResetState()
    {
        visible = false;
        completed = false;
        revealed = false;

        if (contentObject != null)
        {
            contentObject.SetActive(false);

            if (contentPool != null)
                contentObject.transform.SetParent(contentPool, true);
        }

        if (coverObject != null)
        {
            coverObject.SetActive(false);
        }
    }

    public bool IsVisible()
    {
        return visible;
    }

    public bool IsCompleted()
    {
        return completed;
    }

    public bool IsRevealed()
    {
        return revealed;
    }
   
}