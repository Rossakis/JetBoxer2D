namespace Super_Duper_Shooter.Engine.Input.Objects;

/// <summary>
/// Each BaseInputAction will have its own BaseInput, for example MoveHorizontal (BaseInputAction) will have two inputs
/// registered to it, the MoveRight(BaseInput) and MoveLeft(BaseInput)
/// </summary>
public abstract class BaseInput
{
    protected BaseInput()
    {
    }
}