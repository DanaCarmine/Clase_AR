using UnityEngine;
using TMPro;

// Este script guarda el progreso del juego.
// Registra qué amigos ya fueron encontrados y muestra un mensaje en pantalla.
public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance;

    [Header("UI")]
    public TMP_Text infoText;

    public bool rinFound;
    public bool lenFound;
    public bool lukaFound;

    private void Awake()
    {
        Instance = this;
    }

    // Marca un amigo como encontrado
    public void RegisterFriend(string friendName)
    {
        switch (friendName)
        {
            case "Rin":
                rinFound = true;
                break;
            case "Len":
                lenFound = true;
                break;
            case "Luka":
                lukaFound = true;
                break;
        }

        UpdateText(friendName);
    }

    // Muestra mensaje en pantalla
    void UpdateText(string friendName)
    {
        if (infoText != null)
        {
            infoText.text = friendName + " se ha unido al grupo de Miku.";
        }
    }
}