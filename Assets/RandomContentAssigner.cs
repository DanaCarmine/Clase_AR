using System.Collections.Generic;
using UnityEngine;

public class RandomContentAssigner : MonoBehaviour
{
    [System.Serializable]
    public class ContentEntry
    {
        public TargetContentType contentType;
        public string contentName;
        public GameObject contentObject;
    }

    [Header("Marcadores aleatorios")]
    public RandomTargetContent[] targetContents;

    [Header("Contenidos disponibles")]
    public ContentEntry[] availableContents;

    [Header("Pool donde descansan los contenidos")]
    public Transform contentPool;

    private void Start()
    {
        AssignRandomContents();
    }

    public void AssignRandomContents()
    {
        if (targetContents == null || availableContents == null) return;

        if (targetContents.Length != availableContents.Length)
        {
            Debug.LogError("La cantidad de marcadores y contenidos debe ser igual.");
            return;
        }

        foreach (ContentEntry entry in availableContents)
        {
            if (entry != null && entry.contentObject != null)
            {
                entry.contentObject.SetActive(false);

                if (contentPool != null)
                    entry.contentObject.transform.SetParent(contentPool, true);
            }
        }

        List<ContentEntry> shuffled = new List<ContentEntry>(availableContents);

        for (int i = 0; i < shuffled.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffled.Count);
            ContentEntry temp = shuffled[i];
            shuffled[i] = shuffled[randomIndex];
            shuffled[randomIndex] = temp;
        }

        for (int i = 0; i < targetContents.Length; i++)
        {
            targetContents[i].contentPool = contentPool;

            targetContents[i].SetupContent(
                shuffled[i].contentType,
                shuffled[i].contentName,
                shuffled[i].contentObject
            );

            Debug.Log(targetContents[i].gameObject.name + " recibió: " + shuffled[i].contentName);
        }
    }
}