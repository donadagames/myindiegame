public class LandOnWater : IState
{
    private readonly InputHandler inputHandler;

    public LandOnWater(InputHandler _inputHandler)
    {
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
