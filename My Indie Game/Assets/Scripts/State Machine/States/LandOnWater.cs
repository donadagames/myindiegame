public class LandOnWater : IState
{
    private readonly InputHandler inputHandler;
    private readonly Status status;
    public LandOnWater(InputHandler _inputHandler, Status _status)
    {
        inputHandler = _inputHandler;
        status = _status;
    }

    public void OnEnter()
    {
        inputHandler.Splash();
        status.player.soundController.SplashSound();
        inputHandler.SearchForInteractables();

        if (inputHandler.isCarrying)
        {
            inputHandler.carryObject.SetDefaultPosition();
        }

    }


    public void OnExit()
    {
        inputHandler.SetDefaultConfigurations();
    }

    public void Tick()
    {
        inputHandler.GetDirection();
        inputHandler.DetectWater();
    }
}
