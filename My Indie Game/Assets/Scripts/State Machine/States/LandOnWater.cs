public class LandOnWater : IState
{
    private readonly InputHandler inputHandler;
    private readonly Status status;

    public LandOnWater(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        inputHandler.Splash();
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
