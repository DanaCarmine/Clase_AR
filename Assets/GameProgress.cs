using UnityEngine;
using TMPro;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance;

    [Header("UI")]
    public TMP_Text infoText;

    [Header("Objetos desbloqueables")]
    public GameObject microfono;
    public GameObject guitarra;

    [Header("Cambio de pelo")]
    public SkinnedMeshRenderer peloRenderer;
    public Material materialPeloOriginal;
    public Material materialPeloLuka;

    public bool rinFound;
    public bool lenFound;
    public bool lukaFound;
    public bool stageCompleted;

    // 0 = Rin, 1 = Len, 2 = Luka, 3 = Escenario
    private int currentStep = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (microfono != null) microfono.SetActive(false);
        if (guitarra != null) guitarra.SetActive(false);

        if (peloRenderer != null && materialPeloOriginal != null)
        {
            peloRenderer.material = materialPeloOriginal;
        }

        if (infoText != null)
        {
            infoText.text = "Misión: encuentra primero a Rin, después a Len, luego a Luka y por último el escenario.";
        }
    }

    public bool IsExpectedContent(TargetContentType contentType)
    {
        if (currentStep == 0 && contentType == TargetContentType.Rin) return true;
        if (currentStep == 1 && contentType == TargetContentType.Len) return true;
        if (currentStep == 2 && contentType == TargetContentType.Luka) return true;
        if (currentStep == 3 && contentType == TargetContentType.Stage) return true;

        return false;
    }

    public void ShowWrongContentMessage(TargetContentType contentType)
    {
        switch (contentType)
        {
            case TargetContentType.Rin:
                ShowMessage("Rin no es el contenido que debes encontrar ahora.");
                break;

            case TargetContentType.Len:
                if (currentStep == 0)
                    ShowMessage("Len aún no puede unirse. Primero busca a Rin.");
                else
                    ShowMessage("Len no es el contenido que debes encontrar ahora.");
                break;

            case TargetContentType.Luka:
                if (currentStep == 0 || currentStep == 1)
                    ShowMessage("Luka todavía no está lista. Debes encontrar primero a los demás integrantes.");
                else
                    ShowMessage("Luka no es el contenido que debes encontrar ahora.");
                break;

            case TargetContentType.Stage:
                ShowMessage("Aún no puedes iniciar el concierto. Primero reúne a todos los integrantes en el orden correcto.");
                break;
        }
    }

    public bool TryRegisterContent(TargetContentType contentType)
    {
        switch (contentType)
        {
            case TargetContentType.Rin:
                if (currentStep == 0 && !rinFound)
                {
                    rinFound = true;
                    currentStep = 1;

                    if (microfono != null)
                        microfono.SetActive(true);

                    ShowMessage("Rin saludó a Miku y le entregó un micrófono para el concierto. Ahora busca a Len.");
                    return true;
                }
                return false;

            case TargetContentType.Len:
                if (currentStep == 1 && !lenFound)
                {
                    lenFound = true;
                    currentStep = 2;

                    if (guitarra != null)
                        guitarra.SetActive(true);

                    ShowMessage("Len saludó a Miku y le entregó una guitarra para el concierto. Ahora busca a Luka.");
                    return true;
                }
                return false;

            case TargetContentType.Luka:
                if (currentStep == 2 && !lukaFound)
                {
                    lukaFound = true;
                    currentStep = 3;

                    if (peloRenderer != null && materialPeloLuka != null)
                    {
                        peloRenderer.material = materialPeloLuka;
                    }

                    ShowMessage("Luka saludó a Miku y cambió el estilo del cabello de Miku. Ahora busca el escenario.");
                    return true;
                }
                return false;

            case TargetContentType.Stage:
                if (currentStep == 3 && !stageCompleted)
                {
                    stageCompleted = true;
                    currentStep = 4;

                    ShowMessage("¡Todos están listos! Miku llegó al escenario y el concierto puede comenzar.");
                    return true;
                }
                return false;
        }

        return false;
    }

    public bool IsGameCompleted()
    {
        return stageCompleted;
    }

    private void ShowMessage(string message)
    {
        if (infoText != null)
        {
            infoText.text = message;
        }
    }

    public void ResetProgress()
    {
        rinFound = false;
        lenFound = false;
        lukaFound = false;
        stageCompleted = false;

        currentStep = 0;

        if (microfono != null)
            microfono.SetActive(false);

        if (guitarra != null)
            guitarra.SetActive(false);

        if (peloRenderer != null && materialPeloOriginal != null)
        {
            peloRenderer.material = materialPeloOriginal;
        }

        if (infoText != null)
        {
            infoText.text = "Misión: encuentra primero a Rin, después a Len, luego a Luka y por último el escenario.";
        }
    }

    public void ShowResetMessage()
    {
        if (infoText != null)
        {
            infoText.text = "Partida reiniciada. Busca primero a Rin.";
        }
    }
}