public class Floating : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;

    public Floating(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        status.player.sword.shouldCheck = false;

        if (status.uiController.aim != null)
            status.uiController.aim.m_Damping = status.dampingTime;

        inputHandler.SetSwimmingConfiguration();
        inputHandler.isFalling = false;
        inputHandler.jumpCount = 0;
        status.player.animations.PlayAnimation(status.player.animations.FLOAT,
            status.isSafeZone);

        if (inputHandler.isCarrying)
        {
            inputHandler.carryObject.SetDefaultPosition();
        }
    }

    public void OnExit()
    {

        status.uiController.aim.m_Damping = 999999999f;

        inputHandler.SetDefaultConfigurations();
    }

    public void Tick()
    {
        inputHandler.GetDirection();
        inputHandler.DetectWater();
        inputHandler.ApplyAllMovement();
    }
}
