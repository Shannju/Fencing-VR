using UnityEngine;

public class UILVLSelection : MonoBehaviour
{
    public TargetSpawner spawner;

    private TargetSpawner.GameMode selectedMode;
    private TargetSpawner.DifficultyLevel selectedDifficulty;

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

}
