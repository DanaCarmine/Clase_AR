using UnityEngine;

public class GameReset : MonoBehaviour
{
    [Header("Referencias principales")]
    public GameProgress gameProgress;
    public RandomContentAssigner randomContentAssigner;
    public TargetRevealManager targetRevealManager;
    public Move moveScript;
    public ARVisibilityManager arVisibilityManager;
    public FinalMusicController finalMusicController;

    [Header("Marcadores aleatorios")]
    public RandomTargetContent[] randomTargets;

    [Header("Miku")]
    public GameObject mikuModel;
    public Transform initialMarker;

    [Header("Pool de contenidos")]
    public Transform contentPool;

    public void ResetGame()
    {
        if (targetRevealManager != null)
        {
            targetRevealManager.ResetReveal();
        }

        if (finalMusicController != null)
        {
            finalMusicController.StopFinalMusic();
        }

        if (arVisibilityManager != null)
        {
            arVisibilityManager.ResetVisibilityState();
        }

        if (randomTargets != null)
        {
            foreach (RandomTargetContent target in randomTargets)
            {
                if (target != null)
                {
                    target.ForceResetState();
                }
            }
        }

        if (gameProgress != null)
        {
            gameProgress.ResetProgress();
            gameProgress.ShowResetMessage();
        }

        if (mikuModel != null && initialMarker != null)
        {
            mikuModel.transform.SetParent(null, true);
            mikuModel.transform.position = initialMarker.position;
            mikuModel.transform.rotation = initialMarker.rotation;
            mikuModel.transform.SetParent(initialMarker, true);
        }

        if (moveScript != null)
        {
            moveScript.ResetMoveState();
        }

        if (randomContentAssigner != null)
        {
            randomContentAssigner.AssignRandomContents();
        }
    }
}