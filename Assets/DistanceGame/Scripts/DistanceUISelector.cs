using UnityEngine;

public class DistanceUISelector : MonoBehaviour
{
    public DistanceTrainerScript trainer;

    
    public void SelectStaticMove()
    {
        trainer.SetMovementType(DistanceTrainerScript.MovementType.Static);
    }
    public void SelectsineMove()
    {
        trainer.SetMovementType(DistanceTrainerScript.MovementType.Sine);
    }
    public void SelectPingPongMove()
    {
        trainer.SetMovementType(DistanceTrainerScript.MovementType.PingPong);
    }
    public void SelectRandomJumpMove()
    {
        trainer.SetMovementType(DistanceTrainerScript.MovementType.RandomJump);
    }
    public void SelectRandomSmoothMove()
    {
        trainer.SetMovementType(DistanceTrainerScript.MovementType.SmoothRandom);
    }

    
}
