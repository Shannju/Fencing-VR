using UnityEngine;

public class UILVLSelection : MonoBehaviour
{
    public TargetSpawner spawner;

    private TargetSpawner.GameMode selectedMode;
    private TargetSpawner.DifficultyLevel selectedDifficulty;

    public GameObject flatTarget;
    public GameObject modelTarget;


    // --- MODE SELECTION ---

    public void SelectSpeedrun()
    {
        spawner.PrepareLevel(TargetSpawner.GameMode.Speedrun, TargetSpawner.DifficultyLevel.Easy);
    }

    public void SelectEasy()
    {
        spawner.PrepareLevel(TargetSpawner.GameMode.Reaction, TargetSpawner.DifficultyLevel.Easy);
    }

    public void SelectMedium()
    {
        spawner.PrepareLevel(TargetSpawner.GameMode.Reaction, TargetSpawner.DifficultyLevel.Medium);
    }

    public void SelectHard()
    {
        spawner.PrepareLevel(TargetSpawner.GameMode.Reaction, TargetSpawner.DifficultyLevel.Hard);
    }

    public void SelectInferno()
    {
        spawner.PrepareLevel(TargetSpawner.GameMode.Reaction, TargetSpawner.DifficultyLevel.Inferno);
    }

    public void SelectSurvival()
    {
        spawner.PrepareLevel(TargetSpawner.GameMode.Survival, TargetSpawner.DifficultyLevel.Easy);
    }

    // -- Target change --

    public void ChangeTarget()
    {
        if (flatTarget.activeSelf)
        {
            //change to model
            flatTarget.SetActive(false);
            modelTarget.SetActive(true);
        }
        else
        {
            modelTarget.SetActive(false);
            flatTarget.SetActive(true);
            
        }
    }

}
