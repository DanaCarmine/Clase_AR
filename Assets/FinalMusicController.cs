using UnityEngine;

public class FinalMusicController : MonoBehaviour
{
    public static FinalMusicController Instance;

    [Header("Audio")]
    public AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayFinalMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopFinalMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}