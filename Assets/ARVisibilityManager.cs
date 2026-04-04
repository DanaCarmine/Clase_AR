using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ARVisibilityManager : MonoBehaviour
{
    public static ARVisibilityManager Instance;

    [Header("UI")]
    public TMP_Text infoText;

    [Header("Mensajes")]
    [TextArea]
    public string noMarkersMessage = "No se detectan marcadores. Vuelve a colocar un marcador frente a la cámara.";

    private HashSet<string> visibleTargets = new HashSet<string>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterTargetVisible(string targetName)
    {
        if (!string.IsNullOrEmpty(targetName))
        {
            visibleTargets.Add(targetName);
        }
    }

    public void RegisterTargetLost(string targetName)
    {
        if (!string.IsNullOrEmpty(targetName))
        {
            visibleTargets.Remove(targetName);
        }

        CheckNoMarkersVisible();
    }

    public void CheckNoMarkersVisible()
    {
        if (visibleTargets.Count == 0)
        {
            if (infoText != null)
            {
                infoText.text = noMarkersMessage;
            }
        }
    }

    public bool HasVisibleTargets()
    {
        return visibleTargets.Count > 0;
    }

    public void ResetVisibilityState()
    {
        visibleTargets.Clear();
        CheckNoMarkersVisible();
    }
}
